using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Persistence.EntityFramework.Contexts;
using System.Reflection;

namespace PaymentService.DataAccess.Context;

public class PaymentServiceDbContext(DbContextOptions<PaymentServiceDbContext> options, IHttpContextAccessor httpContextAccessor) : CustomDbContext(options, httpContextAccessor)
{
    public const string DEFAULT_SCHEMA = "payments";
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
public class PaymentServiceDbContextFactory
    : IDesignTimeDbContextFactory<PaymentServiceDbContext>
{
    public PaymentServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PaymentServiceDbContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-7TEJ64B;Database=ISAD;Trusted_Connection=true;TrustServerCertificate=true;");

        var httpContextAccessor = new HttpContextAccessor();

        return new PaymentServiceDbContext(optionsBuilder.Options, httpContextAccessor);
    }
}
