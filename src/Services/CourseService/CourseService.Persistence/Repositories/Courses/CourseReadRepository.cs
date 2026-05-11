using CourseService.Applcation.Repositories.Courses;
using CourseService.Domain;
using CourseService.Persistence.Context;
using Persistence.EntityFramework.Repositories;

namespace CourseService.Persistence.Repositories.Courses;

class CourseReadRepository(CourseServiceDbContext dbContext) : ReadRepository<Course>(dbContext), ICourseReadRepository
{
}