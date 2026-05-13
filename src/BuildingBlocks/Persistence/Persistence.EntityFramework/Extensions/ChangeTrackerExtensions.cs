using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

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

            var value = httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier);

            int currentUserId = int.TryParse(value?.Value, out var id) ? id : 0;
            if (entry.Entity is not AuditableEntity auditableEntity)
                continue;

            //if (entry.State == EntityState.Added)
            //{
            //    ((AuditableEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
            //    ((AuditableEntity)entry.Entity).CreatedBy = currentUserId.ToString();
            //}
            if (entry.State == EntityState.Modified)
            {
                ((AuditableEntity)entry.Entity).LastModifiedAt = DateTime.UtcNow;
                ((AuditableEntity)entry.Entity).LastModifiedBy = currentUserId.ToString();
            }
            else if (entry.State == EntityState.Deleted)
            {
                ((AuditableEntity)entry.Entity).DeletedAt = DateTime.UtcNow;
                ((AuditableEntity)entry.Entity).DeletedBy = currentUserId.ToString();
                entry.State = EntityState.Modified;
            }
        }
    }
}