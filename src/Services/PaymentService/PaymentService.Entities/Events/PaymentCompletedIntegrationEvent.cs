using Core.Shared.Models;
using EventBus.Base.Events;

namespace PaymentService.Entities.Events;

public class PaymentCompletedIntegrationEvent : IntegrationEvent
{
    public string CourseId { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public PaymentCompletedIntegrationEvent()
    {
        
    }
    public PaymentCompletedIntegrationEvent(string courseId, bool isSuccess)
    {
        this.CourseId = courseId;
        this.IsSuccess = isSuccess;
    }
}
