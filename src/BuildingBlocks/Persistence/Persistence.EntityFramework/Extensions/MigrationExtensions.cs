using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence.EntityFramework.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync<TContext>(this IServiceProvider services)
        where TContext : DbContext
    {
        using var scope = services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

        await dbContext.Database.MigrateAsync();
    }
}