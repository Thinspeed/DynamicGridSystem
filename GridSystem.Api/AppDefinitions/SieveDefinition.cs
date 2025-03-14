using AppDefinition.Abstractions;
using GridSystem.Api.SievePreferences;
using JetBrains.Annotations;
using Sieve.Models;
using Sieve.Services;

namespace GridSystem.Api.AppDefinitions;

[UsedImplicitly]
public class SieveDefinition : IAppDefinition
{
    public void RegisterDefinition(IHostApplicationBuilder builder)
    {
        builder.Services.Configure<SieveOptions>(builder.Configuration.GetSection("Sieve"));

        builder.Services.AddSingleton<ISieveJsonAccessor, SieveJsonAccessor>();
        builder.Services.AddScoped<AppSieveProcessor>();
    }
}