using EventBus.Base.Events;

namespace EnrollmentService.Domain.Events.IntegrationEvents;


public class CourseEnrolledIntegrationEvent : IntegrationEvent
{
    public Guid CourseId { get; set; } = default!;
    public string CourseName { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public CourseEnrolledIntegrationEvent()
    {

    }
    public CourseEnrolledIntegrationEvent(Guid courseId, string CourseName, int userId, decimal amount)
    {
        this.CourseId = courseId;
        this.CourseName = CourseName;
        this.UserId = userId;
        this.Amount = amount;
    }
}