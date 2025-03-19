using EntityFramework.Configurations.Abstractions;
using GridSystem.Domain.Grids.Columns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Configurations.Grids.Columns;

public class SingleSelectConfiguration : SoftDeletableEntityConfiguration<SingleSelectColumn>
{
    public override void Configure(EntityTypeBuilder<SingleSelectColumn> builder)
    {
    }
}