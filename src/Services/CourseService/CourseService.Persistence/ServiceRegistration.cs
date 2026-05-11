using CourseService.Applcation.Repositories;
using CourseService.Applcation.Repositories.Courses;
using CourseService.Persistence.Context;
using CourseService.Persistence.Repositories;
using CourseService.Persistence.Repositories.Courses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EntityFramework;

namespace CourseService.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CourseServiceDbContext>(conf =>
        {
            conf.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
        });
        services.AddUnitOfWorkTransaction();

        services.AddScoped<ICourseReadRepository, CourseReadRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        return services;
    }
}
