using EventBus.Base.Abstractions;
using EventBus.Base.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace EventBus.Base.Events;

public abstract class BaseEventBus : IEventBus
{
    public readonly IServiceProvider ServiceProvider;
    public readonly IEventBusSubscriptionManager SubManager;

    protected EventBusConfig EventBusConfig { get; private set; }

    protected BaseEventBus(EventBusConfig configuration, IServiceProvider serviceProvider)
    {
        EventBusConfig = configuration;
        ServiceProvider = serviceProvider;
        SubManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }

    public virtual string ProcessEventName(string eventName)
    {
        if (EventBusConfig.RemoveEventPrefix
            && eventName.StartsWith(EventBusConfig.EventNamePrefix, StringComparison.Ordinal))
        {
            eventName = eventName[EventBusConfig.EventNamePrefix.Length..];
        }

        if (EventBusConfig.RemoveEventSuffix
            && eventName.EndsWith(EventBusConfig.EventNameSuffix, StringComparison.Ordinal))
        {
            eventName = eventName[..^EventBusConfig.EventNameSuffix.Length];
        }

        return eventName;
    }

    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);
        var proceeded = false;

        if (SubManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = SubManager.GetHandlersForEvent(eventName);

            using var scope = ServiceProvider.CreateScope();

            foreach (var subscription in subscriptions)
            {
                var handler = scope.ServiceProvider.GetService(subscription.HandlerType);

                if (handler == null) continue;

                var eventType = SubManager.GetEventTypeByName(
                    $"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}");

                var integrationEvent = JsonSerializer.Deserialize(message, eventType);

                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                var handleMethod = concreteType.GetMethod("Handle")
                    ?? throw new InvalidOperationException(
                        $"Handle method not found on {concreteType.Name}");

                await (Task)handleMethod.Invoke(handler, new object[] { integrationEvent! })!;
            }

            proceeded = true;
        }

        return proceeded;
    }

    public abstract Task Publish(IntegrationEvent @event);

    public abstract Task Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    public abstract Task Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    public virtual string GetSubName(string eventName)
        => $"{EventBusConfig.SubscriberAppName}.{ProcessEventName(eventName)}";

    public virtual void Dispose()
    {
        EventBusConfig = null!;
        SubManager.Clear();
    }
}