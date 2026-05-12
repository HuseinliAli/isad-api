using EventBus.Base;
using EventBus.Base.Abstractions;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Messaging.EventHandlers;
using NotificationService.Infrastructure.Messaging.Events;
using NotificationService.Infrastructure.Services;

namespace NotificationService.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentHubService, PaymentHubService>();
        services.AddSingleton<IEventBus>(sp =>
        {
            var eventBusConfig = new EventBusConfig
            {
                ConnectionRetryCount = 5,
                EventNameSuffix = "IntegrationEvent",
                SubscriberAppName = "NotificationService"
            };
            return EventBusFactory.Create(eventBusConfig, sp);
        });
        services.AddTransient<PaymentCompletedIntegrationEventHandler>();
        services.AddTransient<PaymentFailedIntegrationEventHandler>();
        return services;
    }

    public static async Task SubscribeToEvents(this IServiceProvider serviceProvider)
    {
        IEventBus eventBus = serviceProvider.GetRequiredService<IEventBus>();
        await eventBus.Subscribe<PaymentCompletedIntegrationEvent, PaymentCompletedIntegrationEventHandler>();
        await eventBus.Subscribe<PaymentFailedIntegrationEvent, PaymentFailedIntegrationEventHandler>();
    }
}
