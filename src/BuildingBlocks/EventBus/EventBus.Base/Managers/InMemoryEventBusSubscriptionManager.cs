using EventBus.Base.Abstractions;
using EventBus.Base.Events;

namespace EventBus.Base.Managers;

public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<SubscriptionInfo>> handlers;
    private readonly List<Type> eventTypes;

    public event EventHandler<string> OnEventRemoved;
    public Func<string, string> eventNameGetter;
    public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
    {
        handlers = new Dictionary<string, List<SubscriptionInfo>>();
        eventTypes = new List<Type>();
        this.eventNameGetter = eventNameGetter;
    }
    public bool IsEmpty => !handlers.Keys.Any();

    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        string eventName = GetEventKey<T>();

        AddSubscription(typeof(TH), eventName);

        if(!eventTypes.Contains(typeof(T)))
            eventTypes.Add(typeof(T));
    }
    private void AddSubscription(Type handlerType, string eventName)
    {
        if(!HasSubscriptionsForEvent(eventName))
            handlers.Add(eventName, new List<SubscriptionInfo>());

        if (handlers[eventName].Any(s => s.HandlerType == handlerType))
            throw new ArgumentException($"Handler Type: {handlerType.Name} is already used");

        handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
    }
    public void Clear() => handlers.Clear();

    public string GetEventKey<T>()
        => eventNameGetter(typeof(T).Name);

    public Type GetEventTypeByName(string eventName)
        => eventTypes.SingleOrDefault(et => et.Name == eventName);

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent
        => GetHandlersForEvent(GetEventKey<T>());

    public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName)
        => handlers[eventName];

    public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent
        => HasSubscriptionsForEvent(GetEventKey<T>());

    public bool HasSubscriptionsForEvent(string eventName)
        => handlers.ContainsKey(eventName);

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        SubscriptionInfo handlerToRemove = FindSubscriptionToRemove<T, TH>();
        string eventName = GetEventKey<T>();
        RemoveHandler(eventName, handlerToRemove);
    }
    private void RemoveHandler(string eventName, SubscriptionInfo subsToRemove)
    {
        if(subsToRemove != null)
        {
            handlers[eventName].Remove(subsToRemove);

            if (!handlers[eventName].Any())
            {
                handlers.Remove(eventName);
                Type eventType = eventTypes.FirstOrDefault(et => et.Name == eventName);
                
                if(eventType != null)
                    eventTypes.Remove(eventType);

                RaiseOnEventRemoved(eventName);
            }
        }
    }
    private void RaiseOnEventRemoved(string eventName)
    {
        var handler = OnEventRemoved;
        handler?.Invoke(this, eventName);
    }

    private SubscriptionInfo FindSubscriptionToRemove<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
        => FindSubscriptionToRemove(GetEventKey<T>(), typeof(TH));

    private SubscriptionInfo FindSubscriptionToRemove(string eventName, Type handlerType)
    {
        if (!HasSubscriptionsForEvent(eventName)) return null;

        return handlers[eventName].SingleOrDefault(si => si.HandlerType == handlerType);
    }
}
