using EventBus.Base.Events;

namespace NotificationService.Infrastructure.Messaging.Events;

public class PaymentCompletedIntegrationEvent : IntegrationEvent
{
    public string CourseId { get; set; } = default!;
    public string CourseName { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public PaymentCompletedIntegrationEvent()
    {

    }
    public PaymentCompletedIntegrationEvent(string courseId, string courseName, bool isSuccess)
    {
        this.CourseId = courseId;
        this.IsSuccess = isSuccess;
        this.CourseName = courseName;
    }
}
