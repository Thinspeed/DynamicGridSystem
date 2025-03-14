using GridSystem.Api.SievePreferences.Configurations;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace GridSystem.Api.SievePreferences;

public class AppSieveProcessor : SieveProcessor
{
    private ISieveJsonAccessor _sieveJsonAccessor;

    public AppSieveProcessor(IOptions<SieveOptions> options, ISieveJsonAccessor sieveJsonAccessor) 
        : base(options)
    {
        DefaultPageSize = options.Value.DefaultPageSize;
        _sieveJsonAccessor = sieveJsonAccessor;
    }
    
    protected override ISieveJsonAccessor SieveJsonAccessor => _sieveJsonAccessor;
    
    public int DefaultPageSize { get; }

    public int DefaultPage { get; } = 1;

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        // return mapper
        //     .ApplyConfiguration<GridSieveConfiguration>()
        //     .ApplyConfiguration<RowSieveConfiguration>();
        
        return mapper.ApplyConfigurationsFromAssembly(typeof(AppSieveProcessor).Assembly);            
    }
}