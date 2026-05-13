using Core.Domain.Entities;

namespace EnrollmentService.Domain.Snapshots;

public class UnitSnapshot : IEntity
{
    public Guid UnitId { get; set; }
    public string Name { get; set; }

    public List<LessonSnapshot> Lessons { get; set; }
}
