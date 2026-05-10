using Microsoft.Extensions.DependencyInjection;
using Persistence.Base.Abstractions;
using Persistence.EntityFramework.Contexts;

namespace Persistence.EntityFramework;

public static class ServiceRegistration
{
    public static IServiceCollection AddUnitOfWorkTransaction(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CustomDbContext>());
        return services;
    }
}
