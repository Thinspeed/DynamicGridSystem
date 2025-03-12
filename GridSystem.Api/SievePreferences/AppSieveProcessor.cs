using GridSystem.Api.SievePreferences.Configurations;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace GridSystem.Api.SievePreferences;

public class AppSieveProcessor : SieveProcessor
{
    public AppSieveProcessor(IOptions<SieveOptions> options) 
        : base(options)
    {
        DefaultPageSize = options.Value.DefaultPageSize;
    }
    
    public int DefaultPageSize { get; }

    public int DefaultPage { get; } = 1;

    protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
    {
        return mapper
            .ApplyConfiguration<GridSieveConfiguration>();
        
        //return mapper.ApplyConfigurationsFromAssembly(typeof(AppSieveProcessor).Assembly);            
    }
}