using AppDefinition.Abstractions;

namespace GridSystem.Api.AppDefinitions;

public class CorsDefinition : IAppDefinition
{
    public const string CorsPolicyName = "corsDefaultPolicy";
    
    public void RegisterDefinition(IHostApplicationBuilder builder)
    {
        string allowedHosts = builder.Configuration.GetSection("CorsOrigins").Value ?? string.Empty;
        string[] origins = allowedHosts.Split(" ");
        
        builder.Services.AddCors(options => options.AddPolicy(name: CorsPolicyName, policy =>
        {
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }));
    }
}