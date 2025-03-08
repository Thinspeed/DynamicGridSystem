using EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Preferences;

public class ApplicationRwDbContext : DbContext
{
    public ApplicationRwDbContext(DbContextOptions<ApplicationRwDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ConfigurationMarker).Assembly,
                type => type is { IsClass: true, IsAbstract: false });
    }
}