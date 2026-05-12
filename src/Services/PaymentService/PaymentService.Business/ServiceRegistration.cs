using EventBus.Base.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Business.EventHandlers;
using PaymentService.Entities.Events;

namespace PaymentService.Business;

public static class ServiceRegistration 
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CourseEnrolledIntegrationEventHandler>();
        return services;
    }

    public static async Task SubscribeToEvents(this IServiceProvider serviceProvider)
    {
        IEventBus eventBus = serviceProvider.GetRequiredService<IEventBus>();
        await eventBus.Subscribe<CourseEnrolledIntegrationEvent, CourseEnrolledIntegrationEventHandler>();
    }
}