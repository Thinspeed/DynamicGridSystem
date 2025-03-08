using EntityFramework.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Preferences;

public class ApplicationRoDbContext : DbContext
{
    public ApplicationRoDbContext(DbContextOptions<ApplicationRoDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ConfigurationMarker).Assembly,
                type => type is { IsClass: true, IsAbstract: false });
    }
    
    public override int SaveChanges()
    {
        throw new InvalidOperationException("Этот контекст предназначен только для чтения.");
    }
}