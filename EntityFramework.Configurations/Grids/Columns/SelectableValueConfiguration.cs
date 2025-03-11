using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids.Columns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class SingleSelectValueConfiguration : EntityConfiguration<SingleSelectValue>
{
    public override void Configure(EntityTypeBuilder<SingleSelectValue> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(x => x.SingleSelectColumn).WithMany(x => x.Values).HasForeignKey(x => x.SingleSelectColumnId);
    }
}