using EventBus.Base.Abstractions;
using EventBus.Base.Managers;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace EventBus.Base.Events;

public abstract class BaseEventBus : IEventBus
{
    public readonly IServiceProvider ServiceProvider;
    public readonly IEventBusSubscriptionManager SubManager;
    private EventBusConfig configuration;

    protected BaseEventBus(EventBusConfig configuration, IServiceProvider serviceProvider)
    {
        configuration = configuration;
        ServiceProvider = serviceProvider;
        SubManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
    }
    public virtual string ProcessEventName(string eventName)
    {
        if (configuration.RemoveEventPrefix)
            eventName = eventName.TrimStart(configuration.EventNamePrefix.ToArray());

        if(configuration.RemoveEventSuffix)
            eventName = eventName.TrimEnd(configuration.EventNameSuffix.ToArray());

        return eventName;
    }
    public async Task<bool> ProcessEvent(string eventName, string message)
    {
        eventName = ProcessEventName(eventName);
        var proceeded = false;

        if (SubManager.HasSubscriptionsForEvent(eventName))
        {
            var subscriptions = SubManager.GetHandlersForEvent(eventName);

            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var subscription in subscriptions)
                {
                    var handler = ServiceProvider.GetService(subscription.HandlerType);
                    
                    if(handler == null) continue;

                    var eventType = SubManager.GetEventTypeByName($"{configuration.EventNamePrefix}{eventName}{configuration.EventNameSuffix}");
                    var integrationEvent = JsonSerializer.Deserialize(message, eventType);
                    var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                }
            }
            proceeded = true;
        }

        return proceeded;
    }
    public abstract void Publish(IntegrationEvent @event);
    public abstract void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    public abstract void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;
    public virtual string GetSubName(string eventName)
        => $"{configuration.SubscriberAppName}.{ProcessEventName(eventName)}";
    public virtual void Dispose() => configuration = null;
}