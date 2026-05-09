using EventBus.Base;
using EventBus.Base.Events;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace EventBus.RabbitMQ;
public sealed class EventBusRabbitMQ : BaseEventBus
{
    private readonly RabbitMQPersistentConnection persistentConnection;
    private readonly IConnectionFactory connectionFactory;
    private readonly IChannel consumerChannel;

    public EventBusRabbitMQ(EventBusConfig configuration, IServiceProvider serviceProvider)
        : base(configuration, serviceProvider)
    {
        if (configuration.Connection.HasValue)
        {
            var connectionFactory = configuration.Connection.Value.Deserialize<ConnectionFactory>();
            this.connectionFactory = connectionFactory
                ?? throw new InvalidOperationException("Failed to deserialize ConnectionFactory from config.");
        }
        else
        {
            connectionFactory = new ConnectionFactory();
        }

        persistentConnection = new RabbitMQPersistentConnection(
            connectionFactory,
            configuration.ConnectionRetryCount);

        consumerChannel = CreateConsumerChannelAsync().GetAwaiter().GetResult();
        SubManager.OnEventRemoved += SubManager_OnEventRemoved;
    }

    public override async Task Publish(IntegrationEvent @event)
    {
        if (!persistentConnection.IsConnected)
            await persistentConnection.TryConnectAsync();

        var policy = Policy
            .Handle<BrokerUnreachableException>()
            .Or<SocketException>()
            .WaitAndRetryAsync(
                EventBusConfig.ConnectionRetryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (ex, time) =>
                {
                    return Task.CompletedTask;
                });

        var eventName = ProcessEventName(@event.GetType().Name);

        await consumerChannel.ExchangeDeclareAsync(
            exchange: EventBusConfig.DefaultTopicName,
            type: "direct",
            durable: true);

        var message = JsonSerializer.Serialize(@event, @event.GetType());
        var body = Encoding.UTF8.GetBytes(message);

        await policy.ExecuteAsync(async () =>
        {
            var properties = new BasicProperties
            {
                DeliveryMode = DeliveryModes.Persistent
            };

            await consumerChannel.BasicPublishAsync(
                exchange: EventBusConfig.DefaultTopicName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body);
        });
    }

    public override async Task Subscribe<T, TH>()
    {
        var eventName = ProcessEventName(typeof(T).Name);

        if (!SubManager.HasSubscriptionsForEvent(eventName))
        {
            if (!persistentConnection.IsConnected)
                await persistentConnection.TryConnectAsync();

            await consumerChannel.QueueDeclareAsync(
                queue: GetSubName(eventName),
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            await consumerChannel.QueueBindAsync(
                queue: GetSubName(eventName),
                exchange: EventBusConfig.DefaultTopicName,
                routingKey: eventName);
        }

        SubManager.AddSubscription<T, TH>();
        await StartBasicConsumeAsync(eventName);
    }

    public override Task Unsubscribe<T, TH>()
    {
        SubManager.RemoveSubscription<T, TH>();
        return Task.CompletedTask;
    }

    private async Task<IChannel> CreateConsumerChannelAsync()
    {
        if (!persistentConnection.IsConnected)
            await persistentConnection.TryConnectAsync();

        var channel = await persistentConnection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: EventBusConfig.DefaultTopicName,
            type: "direct",
            durable: true);

        return channel;
    }

    private async Task StartBasicConsumeAsync(string eventName)
    {
        if (consumerChannel != null)
        {
            var consumer = new AsyncEventingBasicConsumer(consumerChannel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await consumerChannel.BasicConsumeAsync(
                queue: GetSubName(eventName),
                autoAck: false,
                consumer: consumer);
        }
    }

    private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var eventName = ProcessEventName(eventArgs.RoutingKey);
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        try
        {
            await ProcessEvent(eventName, message);
            await consumerChannel.BasicAckAsync(eventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception)
        {
            await consumerChannel.BasicNackAsync(
                eventArgs.DeliveryTag,
                multiple: false,
                requeue: true);
            throw;
        }
    }

    private async void SubManager_OnEventRemoved(object sender, string eventName)
    {
        eventName = ProcessEventName(eventName);

        if (!persistentConnection.IsConnected)
            await persistentConnection.TryConnectAsync();

        await consumerChannel.QueueUnbindAsync(
            queue: GetSubName(eventName),
            exchange: EventBusConfig.DefaultTopicName,
            routingKey: eventName);

        if (SubManager.IsEmpty)
            await consumerChannel.CloseAsync();
    }
}