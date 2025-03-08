using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class ColumnConfiguration : EntityConfiguration<Column>
{
    public override void Configure(EntityTypeBuilder<Column> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(x => x.Grid).WithMany(x => x.Columns).HasForeignKey(x => x.GridId);
        
        builder.Ignore(x => x.Type);
    }
}