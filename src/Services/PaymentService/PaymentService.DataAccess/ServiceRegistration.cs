using EventBus.Base;
using EventBus.Base.Abstractions;
using EventBus.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.DataAccess.Context;
using PaymentService.DataAccess.Repositories.Abstractions;
using PaymentService.DataAccess.Repositories.Concretes;
using Persistence.EntityFramework;

namespace PaymentService.DataAccess;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PaymentServiceDbContext>(conf =>
        {
            conf.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
        });
        services.AddUnitOfWorkTransaction<PaymentServiceDbContext>();
        services.AddSingleton<IEventBus>(sp =>
        {
            var eventBusConfig = new EventBusConfig
            {
                ConnectionRetryCount = 5,
                EventNameSuffix = "IntegrationEvent",
                SubscriberAppName = "PaymentService"
            };  
            return EventBusFactory.Create(eventBusConfig, sp);
        });
    

        services.AddScoped<IPaymentReadRepository, PaymentReadRepository>();
        services.AddScoped<IPaymentWriteRepository, PaymentWriteRepository>();
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        return services;
    }
}
