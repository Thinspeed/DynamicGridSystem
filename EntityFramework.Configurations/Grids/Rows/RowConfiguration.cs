using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids.Rows;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Rows;

public class RowConfiguration : EntityConfiguration<Row>
{
    public override void Configure(EntityTypeBuilder<Row> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(x => x.Grid).WithMany(x => x.Rows).HasForeignKey(x => x.GridId);

        builder.Property(x => x.Data).HasColumnType("jsonb");
        
        builder.Ignore(x => x.DataAsString);
    }
}