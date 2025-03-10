using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class StringColumnConfiguration : EntityConfiguration<StringColumn>
{
    public override void Configure(EntityTypeBuilder<StringColumn> builder)
    {
    }
}