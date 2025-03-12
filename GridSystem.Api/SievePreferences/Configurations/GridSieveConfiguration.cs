using GridSystem.Domain.Grids;
using Sieve.Services;

namespace GridSystem.Api.SievePreferences.Configurations;

public class GridSieveConfiguration : ISieveConfiguration
{
    public void Configure(SievePropertyMapper mapper)
    {
        mapper.Property<Grid>(p => p.Name)
            .CanFilter()
            .CanSort();
    }
}