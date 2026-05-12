using Core.Shared.Models;
using EventBus.Base.Events;

namespace PaymentService.Entities.Events;

public class CourseEnrolledIntegrationEvent : IntegrationEvent
{
    public string CourseId { get; set; } = default!;
    public string CourseName { get; set; }  
    public decimal Amount { get; set; }
    public CourseEnrolledIntegrationEvent()
    {
        
    }
    public CourseEnrolledIntegrationEvent(string courseId, string CourseName, decimal amount)
    {
        this.CourseId = courseId;
        this.CourseName = CourseName;
        this.Amount = amount;
    }
}