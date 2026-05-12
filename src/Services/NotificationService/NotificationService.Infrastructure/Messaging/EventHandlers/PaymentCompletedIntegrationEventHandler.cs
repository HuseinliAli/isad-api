using EventBus.Base.Abstractions;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Messaging.Events;

namespace NotificationService.Infrastructure.Messaging.EventHandlers;

public class PaymentCompletedIntegrationEventHandler(IPaymentHubService notificationService) : IIntegrationEventHandler<PaymentCompletedIntegrationEvent>
{
    public async Task Handle(PaymentCompletedIntegrationEvent @event)
    {
        await notificationService.SendPaymentSuccededMessageAsync(@event.CourseName);
    }
}
