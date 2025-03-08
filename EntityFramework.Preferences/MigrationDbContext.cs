using EntityFramework.Configurations;
using GridSystem.Domain.Grids;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Preferences;

public class MigrationDbContext : DbContext
{
    public MigrationDbContext(DbContextOptions<MigrationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ConfigurationMarker).Assembly,
                type => type is { IsClass: true, IsAbstract: false });
    }
}