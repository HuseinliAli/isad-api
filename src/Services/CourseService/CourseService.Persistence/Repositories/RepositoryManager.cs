using CourseService.Applcation.Repositories;
using CourseService.Applcation.Repositories.Courses;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Base.Abstractions;

namespace CourseService.Persistence.Repositories;

internal class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<ICourseReadRepository> courseReadRepository;
    private readonly Lazy<IUnitOfWork> unitOfWork;

    public RepositoryManager(IServiceProvider sp)
    {
        courseReadRepository = new Lazy<ICourseReadRepository>(sp.GetRequiredService<ICourseReadRepository>);
        unitOfWork = new Lazy<IUnitOfWork>(sp.GetRequiredService<IUnitOfWork>);
    }

    public ICourseReadRepository CourseRead => courseReadRepository.Value;
    public IUnitOfWork UnitOfWork => unitOfWork.Value;
}