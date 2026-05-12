using EventBus.Base.Abstractions;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Messaging.Events;

namespace NotificationService.Infrastructure.Messaging.EventHandlers;

public class PaymentFailedIntegrationEventHandler(IPaymentHubService notificationService) : IIntegrationEventHandler<PaymentFailedIntegrationEvent>
{
    public async Task Handle(PaymentFailedIntegrationEvent @event)
    {
        await notificationService.SendPaymentFailedMessageAsync(@event.CourseName, @event.Reason);
    }
}