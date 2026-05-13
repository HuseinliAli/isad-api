using Core.Domain.Entities;

namespace EnrollmentService.Domain;

public class Enrollment : AuditableEntity
{
    public Guid Id { get; set; }

    public int UserId { get; set; }
    public Guid CourseId { get; set; }

    public DateTime EnrolledAt { get; set; }

    public decimal ProgressPercentage { get; set; }

    public ICollection<LessonProgress> LessonProgresses { get; set; }
}
