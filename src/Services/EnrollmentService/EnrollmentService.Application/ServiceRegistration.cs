using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace EnrollmentService.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));
        return services;
    }
}
