using EventBus.Base.Abstractions;
using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Concretes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Business.Abstractions;
using PaymentService.Business.Concretes;
using PaymentService.Business.EventHandlers;
using PaymentService.Entities.Events;

namespace PaymentService.Business;

public static class ServiceRegistration 
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<CourseEnrolledIntegrationEventHandler>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IPaymentHistoryService, PaymentHistoryService>();
        return services;
    }

    public static async Task SubscribeToEvents(this IServiceProvider serviceProvider)
    {
        IEventBus eventBus = serviceProvider.GetRequiredService<IEventBus>();
        await eventBus.Subscribe<CourseEnrolledIntegrationEvent, CourseEnrolledIntegrationEventHandler>();
    }
}