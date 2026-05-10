using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence.Base.Abstractions;
using Persistence.EntityFramework.Extensions;

namespace Persistence.EntityFramework.Contexts;

public class CustomDbContext : DbContext, IUnitOfWork
{
    protected IHttpContextAccessor httpContextAccessor;

    public CustomDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ChangeTracker.SetAuditProperties(httpContextAccessor);
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.SetAuditProperties(httpContextAccessor);
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        ChangeTracker.SetAuditProperties(httpContextAccessor);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChangeTracker.SetAuditProperties(httpContextAccessor);
        return base.SaveChangesAsync(cancellationToken);
    }
}