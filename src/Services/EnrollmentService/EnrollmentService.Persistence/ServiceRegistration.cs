using EnrollmentService.Application.Repositories;
using EnrollmentService.Application.Repositories.CourseSnapshots;
using EnrollmentService.Application.Repositories.Enrollments;
using EnrollmentService.Persistence.Context;
using EnrollmentService.Persistence.Repositories;
using EnrollmentService.Persistence.Repositories.CourseSnapshots;
using EnrollmentService.Persistence.Repositories.Enrollments;
using EventBus.Base;
using EventBus.Base.Abstractions;
using EventBus.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EntityFramework;
namespace EnrollmentService.Persistence;

public static class ServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EnrollmentServiceDbContext>(conf =>
        {
            conf.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
        });
        services.AddUnitOfWorkTransaction<EnrollmentServiceDbContext>();

        services.AddSingleton<IEventBus>(sp =>
        {
            var eventBusConfig = new EventBusConfig
            {
                ConnectionRetryCount = 5,
                EventNameSuffix = "IntegrationEvent",
                SubscriberAppName = "EnrollmentService"
            };
            return EventBusFactory.Create(eventBusConfig, sp);
        });

        services.AddScoped<IEnrollmentReadRepository, EnrollmentReadRepository>();
        services.AddScoped<IEnrollmentWriteRepository, EnrollmentWriteRepository>();
        services.AddScoped<ICourseSnapshotReadRepository, CourseSnapshotReadRepository>();
        services.AddScoped<ICourseSnapshotWriteRepository, CourseSnapshotWriteRepository>();
        services.AddScoped<IRepositoryManager,RepositoryManager>();

        return services;
    }
}
