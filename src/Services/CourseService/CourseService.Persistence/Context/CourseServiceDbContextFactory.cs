using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CourseService.Persistence.Context;

public class CourseServiceDbContextFactory
    : IDesignTimeDbContextFactory<CourseServiceDbContext>
{
    public CourseServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CourseServiceDbContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-7TEJ64B;Database=ISAD;Trusted_Connection=true;TrustServerCertificate=true;");

        var httpContextAccessor = new HttpContextAccessor();

        return new CourseServiceDbContext(optionsBuilder.Options, httpContextAccessor);
    }
}
