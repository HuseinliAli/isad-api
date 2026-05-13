using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework.Contexts;

namespace EnrollmentService.Persistence.Context;

public class EnrollmentServiceDbContext(DbContextOptions<EnrollmentServiceDbContext> options, IHttpContextAccessor httpContextAccessor) : CustomDbContext(options, httpContextAccessor)
{
    public const string DEFAULT_SCHEMA = "enrollments";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EnrollmentServiceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}