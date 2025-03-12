using System.Reflection;
using GridSystem.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EntityFramework.Preferences.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
        foreach (var entry in eventData.Context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: SoftDeletableEntity delete }) continue;

            IEntityType entityType = entry.Metadata;
            IEnumerable<IForeignKey> navigationProperties = entityType.GetReferencingForeignKeys();
            
            //todo переписать логику взятия и сравнения PK(может нужно получить все FK сразу после запуска)
            foreach (var foreignKey in navigationProperties)
            {
                IEntityType relatedEntityType = foreignKey.DeclaringEntityType;
                MethodInfo method = typeof(DbContext).GetMethod("Set")!.MakeGenericMethod(relatedEntityType.ClrType);
                object? dbSet = method.Invoke(eventData.Context, null);

                if (dbSet is IQueryable queryable)
                {
                    var hasReferences = queryable
                        .Cast<object>()
                        .Any(e => EF.Property<object>(e, foreignKey.Properties.First().Name) == entry.Property("Id").CurrentValue);

                    if (hasReferences)
                    {
                        throw new InvalidOperationException($"Невозможно удалить {entityType.Name}, так как на него ссылаются другие записи.");
                    }
                }
            }
            
            entry.State = EntityState.Modified;
            delete.IsDeleted = true;
            delete.DeletedAt = DateTime.Now;
        }
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}