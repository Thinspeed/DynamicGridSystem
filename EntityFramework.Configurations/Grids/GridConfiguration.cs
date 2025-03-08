using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids;

public class GridConfiguration : EntityConfiguration<Grid>
{
    public override void Configure(EntityTypeBuilder<Grid> builder)
    {
        base.Configure(builder);

        builder.Navigation(x => x.Columns).UsePropertyAccessMode(PropertyAccessMode.Property);
        builder.Navigation(x => x.Rows).UsePropertyAccessMode(PropertyAccessMode.Property);
    }
}