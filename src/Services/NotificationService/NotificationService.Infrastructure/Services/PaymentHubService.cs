using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Services;
using NotificationService.Infrastructure.Hubs;

namespace NotificationService.Infrastructure.Services;

public class PaymentHubService(IHubContext<PaymentNotificationHub> hub) : IPaymentHubService
{
    public Task SendPaymentSuccededMessageAsync(string course)
        => hub.Clients.All.SendAsync("PaymentCompleted", course);

    public Task SendPaymentFailedMessageAsync(string course, string reason)
        => hub.Clients.All.SendAsync("PaymentFailed", new { course, reason });
}