using AppDefinition.Abstractions;
using JetBrains.Annotations;

namespace GridSystem.Api.AppDefinitions;

[UsedImplicitly]
public class MediatrDefinition : IAppDefinition
{
    public void RegisterDefinition(IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MediatrDefinition).Assembly));
    }
}