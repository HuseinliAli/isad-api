using EventBus.Base.Events;

namespace NotificationService.Infrastructure.Messaging.Events;

public class PaymentFailedIntegrationEvent : IntegrationEvent
{
    public string CourseId { get; set; } = default!;
    public string CourseName { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public PaymentFailedIntegrationEvent()
    {

    }
    public PaymentFailedIntegrationEvent(string courseId, string courseName, string reason)
    {
        this.CourseId = courseId;
        this.CourseName = courseName;
        this.Reason = reason;
    }
}