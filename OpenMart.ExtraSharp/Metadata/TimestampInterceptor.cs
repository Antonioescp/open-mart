using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace OpenMart.ExtraSharp.Metadata;

public class TimestampInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is not null)
        {
            UpdateTimestamps(eventData.Context.ChangeTracker.Entries());
        }
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateTimestamps(eventData.Context.ChangeTracker.Entries());
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateTimestamps(IEnumerable<EntityEntry> entries)
    {
        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            var entityType = entry.Metadata.ClrType;

            // Check if the entity has CreatedAt and UpdatedAt properties
            var createdAtProperty = entityType.GetProperty("CreatedAt");
            var updatedAtProperty = entityType.GetProperty("UpdatedAt");

            // If both CreatedAt and UpdatedAt properties exist, update them
            if (createdAtProperty == null || updatedAtProperty == null)
            {
                continue;
            }
            
            var now = DateTime.UtcNow;
            var targetProperty = entry.State switch
            {
                EntityState.Added when createdAtProperty.CanWrite => createdAtProperty,
                EntityState.Modified when updatedAtProperty.CanWrite => updatedAtProperty,
                _ => null
            };
            
            targetProperty?.SetValue(entity, now);
        }
    }
}