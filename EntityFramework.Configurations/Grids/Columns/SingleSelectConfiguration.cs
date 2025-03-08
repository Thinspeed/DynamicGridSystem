using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class SingleSelectConfiguration : EntityConfiguration<SingleSelectColumn>
{
    public override void Configure(EntityTypeBuilder<SingleSelectColumn> builder)
    {
        //base.Configure(builder);

        builder.Property(x => x.Values)
            .HasColumnType("jsonb");
    }
}