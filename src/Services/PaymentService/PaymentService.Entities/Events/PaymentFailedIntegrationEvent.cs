using Core.Shared.Models;
using EventBus.Base.Events;

namespace PaymentService.Entities.Events;

public class PaymentFailedIntegrationEvent : IntegrationEvent
{
    public string CourseId { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public PaymentFailedIntegrationEvent()
    {
        
    }
    public PaymentFailedIntegrationEvent(string courseId, string reason)
    {
        this.CourseId = courseId;
        this.Reason = reason;
    }
}