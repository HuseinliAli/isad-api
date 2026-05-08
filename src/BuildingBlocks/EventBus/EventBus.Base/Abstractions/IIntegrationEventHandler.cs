using EventBus.Base.Events;

namespace EventBus.Base.Abstractions;

public interface IIntegrationEventHandler<TIntegrationEvent> : IntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{

    Task Handle(TIntegrationEvent @event);
}