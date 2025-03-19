using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids.Columns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class NumericColumnConfiguration : SoftDeletableEntityConfiguration<NumericColumn>
{
    public override void Configure(EntityTypeBuilder<NumericColumn> builder)
    {
    }
}