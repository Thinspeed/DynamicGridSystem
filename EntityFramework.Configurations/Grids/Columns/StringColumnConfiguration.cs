using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids.Columns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class StringColumnConfiguration : SoftDeletableEntityConfiguration<StringColumn>
{
    public override void Configure(EntityTypeBuilder<StringColumn> builder)
    {
    }
}