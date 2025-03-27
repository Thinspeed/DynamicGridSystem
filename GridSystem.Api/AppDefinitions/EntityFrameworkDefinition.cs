using AppDefinition.Abstractions;
using EntityFramework.Preferences;
using EntityFramework.Preferences.Interceptors;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace GridSystem.Api.AppDefinitions;

[UsedImplicitly]
public class EntityFrameworkDefinition : IAppDefinition
{
    public void RegisterDefinition(IHostApplicationBuilder builder)
    {
        string rwConnectionString = builder.Configuration.GetSection("ReadWriteConnectionString").Value 
                                    ?? throw new InvalidOperationException("Read/write connection string was not provided");

        string roConnectionString = builder.Configuration.GetSection("ReadOnlyConnectionString").Value 
                                    ?? throw new InvalidOperationException("Read only connection string was not provided");
        
        string migrationConnectionString = builder.Configuration.GetSection("MigrationConnectionString").Value 
                                           ?? throw new InvalidOperationException("Migration connection string was not provided");
        
        builder.Services.AddDbContext<ApplicationRwDbContext>(options => options
            .UseNpgsql(rwConnectionString)
            .AddInterceptors(new SoftDeleteInterceptor()));

        builder.Services.AddDbContext<ApplicationRoDbContext>(options => options
            .UseNpgsql(roConnectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        
        builder.Services.AddDbContext<MigrationDbContext>(options => options
            .UseNpgsql(migrationConnectionString));
    }
    
    public void Init(IHost app)
    {
        IServiceScope scope = app.Services.CreateScope();
        ILogger<Program>? logger = scope.ServiceProvider.GetService<ILogger<Program>>();
        
        using var dbContext = scope.ServiceProvider.GetRequiredService<MigrationDbContext>();
        
        logger?.LogInformation("Применение миграций...");
        
        dbContext.Database.Migrate();
        
        logger?.LogInformation("Миграции применены."); 
        
        scope.Dispose();
    }
}