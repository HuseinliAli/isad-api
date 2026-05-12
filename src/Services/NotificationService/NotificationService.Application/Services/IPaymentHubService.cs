namespace NotificationService.Application.Services;

public interface IPaymentHubService
{
    Task SendPaymentSuccededMessageAsync(string course);
    Task SendPaymentFailedMessageAsync(string course, string reason);
}
