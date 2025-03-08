using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Rows;

public class RowConfiguration : EntityConfiguration<Row>
{
    public override void Configure(EntityTypeBuilder<Row> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(x => x.Grid).WithMany().HasForeignKey(x => x.GridId);

        builder.Property(x => x.Data).HasColumnType("jsonb");
    }
}