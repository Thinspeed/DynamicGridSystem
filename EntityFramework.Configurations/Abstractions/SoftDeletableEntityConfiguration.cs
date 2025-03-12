using GridSystem.Domain.Abstractions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Abstractions;

public abstract class SoftDeletableEntityConfiguration<T> : EntityConfiguration<T>
    where T : SoftDeletableEntity
{
    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}