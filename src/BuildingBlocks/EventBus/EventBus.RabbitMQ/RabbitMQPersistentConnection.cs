using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace EventBus.RabbitMQ;

public class RabbitMQPersistentConnection : IDisposable
{
    private IConnectionFactory connectionFactory;
    private readonly int retryCount;
    private IConnection connection;
    private readonly object lock_object = new object();
    private bool disposed;

    public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
    {
        this.connectionFactory = connectionFactory;
        this.retryCount = retryCount;
    }

    public bool IsConnected => connection != null && connection.IsOpen;
    public async Task<IChannel> CreateChannelAsync()
        => await connection.CreateChannelAsync();
    public void Dispose()
    {
        disposed = true;
        connection.Dispose();
    }

    public async Task<bool> TryConnectAsync()
    {
        lock (lock_object)
            if (IsConnected) 
                return true;

        var policy = Policy.Handle<SocketException>()
            .Or<BrokerUnreachableException>()
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
            {

            });

        await policy.ExecuteAsync(async () =>
        {
            connection?.Dispose();

            connection = await connectionFactory.CreateConnectionAsync();
        });

        if (!IsConnected) 
            return false;

        connection.ConnectionShutdownAsync += Connection_ConnectionShutdown;
        connection.CallbackExceptionAsync += Connection_CallbackException;
        connection.ConnectionBlockedAsync += Connection_ConnectionBlocked;
        

        return true;
}

    private async Task Connection_ConnectionBlocked(object sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
    {
        if (disposed) 
            return;

        await TryConnectAsync();
    }

    private async Task Connection_CallbackException(object sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
    {
        if (disposed) 
            return;

        await TryConnectAsync();
    }

    private async Task Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
        if (disposed) 
            return;

        await TryConnectAsync();
    }
}