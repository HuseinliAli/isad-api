using IdentityService.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.DataAccess.Context;

public class IdentityServiceDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options): base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }
}
