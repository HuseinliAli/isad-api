using EnrollmentService.Application.Repositories.CourseSnapshots;
using EnrollmentService.Application.Repositories.Enrollments;
using Persistence.Base.Abstractions;

namespace EnrollmentService.Application.Repositories;

public interface IRepositoryManager
{
    public IEnrollmentReadRepository EnrollmentRead { get; }
    public IEnrollmentWriteRepository EnrollmentWrite { get; }
    public ICourseSnapshotReadRepository CourseSnapshotRead { get; }
    public ICourseSnapshotWriteRepository CourseSnapshotWrite { get; }
    public IUnitOfWork UnitOfWork { get; }
}