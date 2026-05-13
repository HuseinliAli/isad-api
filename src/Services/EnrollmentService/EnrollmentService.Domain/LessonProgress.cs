using Core.Domain.Entities;

namespace EnrollmentService.Domain;

public class LessonProgress : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
}