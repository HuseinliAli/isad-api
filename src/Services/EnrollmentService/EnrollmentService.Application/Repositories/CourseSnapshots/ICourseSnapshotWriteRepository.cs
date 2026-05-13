using EnrollmentService.Domain.Snapshots;
using Persistence.Base.Abstractions;

namespace EnrollmentService.Application.Repositories.CourseSnapshots;

public interface ICourseSnapshotWriteRepository : IWriteRepository<CourseSnapshot>
{
}   