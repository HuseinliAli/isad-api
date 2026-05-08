using EventBus.Base;
using EventBus.Base.Abstractions;
using EventBus.RabbitMQ;

namespace EventBus.Factory;

public static class EventBusFactory
{
    public static IEventBus Create(EventBusConfig config, IServiceProvider serviceProvider)
    {
        return new EventBusRabbitMQ(config, serviceProvider);
    }
}
