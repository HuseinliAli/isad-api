using EnrollmentService.Application.Repositories;
using EnrollmentService.Application.Repositories.CourseSnapshots;
using EnrollmentService.Application.Repositories.Enrollments;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Base.Abstractions;

namespace EnrollmentService.Persistence.Repositories;

internal class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IEnrollmentReadRepository> enrollmentReadRepository;
    private readonly Lazy<IEnrollmentWriteRepository> enrollmentWriteRepository;
    private readonly Lazy<ICourseSnapshotReadRepository> courseSnapshotReadRepository;
    private readonly Lazy<ICourseSnapshotWriteRepository> courseSnapshotWriteRepository;
    private readonly Lazy<IUnitOfWork> unitOfWork;

    public RepositoryManager(IServiceProvider sp)
    {
        enrollmentReadRepository = new Lazy<IEnrollmentReadRepository>(sp.GetRequiredService<IEnrollmentReadRepository>);
        enrollmentWriteRepository = new Lazy<IEnrollmentWriteRepository>(sp.GetRequiredService<IEnrollmentWriteRepository>);
        courseSnapshotReadRepository = new Lazy<ICourseSnapshotReadRepository>(sp.GetRequiredService<ICourseSnapshotReadRepository>);
        courseSnapshotWriteRepository = new Lazy<ICourseSnapshotWriteRepository>(sp.GetRequiredService<ICourseSnapshotWriteRepository>);
        unitOfWork = new Lazy<IUnitOfWork>(sp.GetRequiredService<IUnitOfWork>);
    }

    public IEnrollmentReadRepository EnrollmentRead => enrollmentReadRepository.Value;
    public IEnrollmentWriteRepository EnrollmentWrite => enrollmentWriteRepository.Value;
    public ICourseSnapshotReadRepository CourseSnapshotRead => courseSnapshotReadRepository.Value;
    public ICourseSnapshotWriteRepository CourseSnapshotWrite => courseSnapshotWriteRepository.Value;
    public IUnitOfWork UnitOfWork => unitOfWork.Value;
}