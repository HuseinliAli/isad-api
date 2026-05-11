using CourseService.Applcation.Repositories.Courses;
using Persistence.Base.Abstractions;

namespace CourseService.Applcation.Repositories;

public interface IRepositoryManager
{
    public ICourseReadRepository CourseRead { get; }
    public IUnitOfWork UnitOfWork { get; }
}