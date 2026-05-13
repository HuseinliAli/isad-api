using Core.Domain.Entities;

namespace EnrollmentService.Domain.Snapshots;

public class CourseSnapshot : IEntity
{
    public string CourseId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public List<UnitSnapshot> Units { get; set; }
}