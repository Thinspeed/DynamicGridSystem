using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids.Columns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class ColumnConfiguration : SoftDeletableEntityConfiguration<Column>
{
    public override void Configure(EntityTypeBuilder<Column> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(x => x.Grid).WithMany(x => x.Columns).HasForeignKey(x => x.GridId);
        
        builder.Ignore(x => x.Type);
    }
}