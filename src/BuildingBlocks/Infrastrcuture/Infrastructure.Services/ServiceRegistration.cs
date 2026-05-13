using Infrastructure.Services.Abstractions;
using Infrastructure.Services.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
