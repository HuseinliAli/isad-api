using Core.Domain.Entities;

namespace EnrollmentService.Domain.Snapshots;

public class LessonSnapshot : IEntity
{
    public Guid LessonId { get; set; }
    public string Name { get; set; }
}
