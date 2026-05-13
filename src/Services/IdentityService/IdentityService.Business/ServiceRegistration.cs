using FluentValidation;
using IdentityService.Business.Abstractions;
using IdentityService.Business.Concretes;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityService.Business;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IBusinessReference>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
public interface IBusinessReference;