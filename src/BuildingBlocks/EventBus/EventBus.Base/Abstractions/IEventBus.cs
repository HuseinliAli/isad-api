using EventBus.Base.Events;

namespace EventBus.Base.Abstractions;

public interface IEventBus
{
    Task Publish(IntegrationEvent @event);

    Task Subscribe<T, TH>()
        where T: IntegrationEvent
        where TH: IIntegrationEventHandler<T>;

    Task Unsubscribe<T, TH>()
        where T: IntegrationEvent
        where TH: IIntegrationEventHandler<T>;
}