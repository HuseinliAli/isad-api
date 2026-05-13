using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EnrollmentService.Persistence.Context;

public class EnrollmentServiceDbContextFactory
    : IDesignTimeDbContextFactory<EnrollmentServiceDbContext>
{
    public EnrollmentServiceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EnrollmentServiceDbContext>();
        optionsBuilder.UseSqlServer("Server=DESKTOP-7TEJ64B;Database=ISAD;Trusted_Connection=true;TrustServerCertificate=true;");

        var httpContextAccessor = new HttpContextAccessor();

        return new EnrollmentServiceDbContext(optionsBuilder.Options, httpContextAccessor);
    }
}
