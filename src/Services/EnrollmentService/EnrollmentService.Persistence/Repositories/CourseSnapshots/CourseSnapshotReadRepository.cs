using EnrollmentService.Application.Repositories.CourseSnapshots;
using EnrollmentService.Domain.Snapshots;
using EnrollmentService.Persistence.Context;
using Persistence.EntityFramework.Repositories;

namespace EnrollmentService.Persistence.Repositories.CourseSnapshots;

internal class CourseSnapshotReadRepository(EnrollmentServiceDbContext dbContext) : ReadRepository<CourseSnapshot>(dbContext),ICourseSnapshotReadRepository
{
}
