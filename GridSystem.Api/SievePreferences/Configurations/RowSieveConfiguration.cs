using GridSystem.Domain.Grids.Rows;
using Sieve.Services;

namespace GridSystem.Api.SievePreferences.Configurations;

public class RowSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Row>(x => x.Data)
            .CanSort()
            .CanFilter();
    }
}