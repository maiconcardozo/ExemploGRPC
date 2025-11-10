using ExemploGRPC.Domain.Entities.Implementation;
using ExemploGRPC.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ExemploGRPC.Infrastructure.Data;

/// <summary>
/// Application database context
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Cargo> Cargos => Set<Cargo>();
    public DbSet<ClienteCargo> ClienteCargos => Set<ClienteCargo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations
        modelBuilder.ApplyConfiguration(new ClienteConfiguration());
        modelBuilder.ApplyConfiguration(new CargoConfiguration());
        modelBuilder.ApplyConfiguration(new ClienteCargoConfiguration());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Update timestamps automatically
        foreach (var entry in ChangeTracker.Entries<Domain.Entities.Base.EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.MarkAsUpdated();
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
