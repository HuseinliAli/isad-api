using EventBus.Base;
using EventBus.Base.Abstractions;
using EventBus.Base.Events;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.UnitTest
{
    public class RabbitMQTests
    {
        [Fact]
        public void subscribe_event_on_rabbitmq()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetEventBusConfig(), sp);
            });

            var sp = services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            eventBus.Unsubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }

        [Fact]
        public void send_message_to_rabbitmq()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IEventBus>(sp =>
            {
               

                return EventBusFactory.Create(GetEventBusConfig(), sp);
            });

            var sp = services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();
            eventBus.Publish(new OrderCreatedIntegrationEvent()).GetAwaiter();
        }

        private EventBusConfig GetEventBusConfig()
        {
            EventBusConfig config = new()
            {
                ConnectionRetryCount = 5,
                SubscriberAppName = "EventBus.UnitTest",
                DefaultTopicName = "TestTopic",
                EventNameSuffix = "IntegrationEvent"
            };

            return config;
        }

        public class OrderCreatedIntegrationEvent : IntegrationEvent;
        public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
        {
            public Task Handle(OrderCreatedIntegrationEvent @event) => Task.CompletedTask;
        }
    }
}
