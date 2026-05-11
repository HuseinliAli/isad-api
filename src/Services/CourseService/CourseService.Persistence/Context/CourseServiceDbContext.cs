using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.EntityFramework.Contexts;
using System.Reflection;

namespace CourseService.Persistence.Context;
public class CourseServiceDbContext(DbContextOptions<CourseServiceDbContext> options, IHttpContextAccessor httpContextAccessor) : CustomDbContext(options, httpContextAccessor)
{
    public const string DEFAULT_SCHEMA = "courses";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}