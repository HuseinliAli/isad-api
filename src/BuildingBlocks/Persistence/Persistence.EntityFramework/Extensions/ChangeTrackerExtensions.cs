using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Persistence.EntityFramework.Extensions;


public static class ChangeTrackerExtensions
{
    public static void SetAuditProperties(this ChangeTracker changeTracker, IHttpContextAccessor httpContextAccessor)
    {
        var entries = changeTracker.Entries();

        SaveAsAuditable(entries, httpContextAccessor);
    }

    public static void SaveAsAuditable(IEnumerable<EntityEntry> entries, IHttpContextAccessor httpContextAccessor)
    {
        foreach (var entry in entries)
        {
            if (entry.Entity is not AuditableEntity auditableEntity)
                continue;

            if (entry.State == EntityState.Added)
            {
                ((AuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                ((AuditableEntity)entry.Entity).CreatedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
            else if (entry.State == EntityState.Modified)
            {
                ((AuditableEntity)entry.Entity).LastModifiedAt = DateTime.UtcNow;
                ((AuditableEntity)entry.Entity).LastModifiedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
            else if (entry.State == EntityState.Deleted)
            {
                ((AuditableEntity)entry.Entity).DeletedAt = DateTime.UtcNow;
                ((AuditableEntity)entry.Entity).DeletedBy = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Unknown";
                entry.State = EntityState.Modified;
            }
        }
    }
}