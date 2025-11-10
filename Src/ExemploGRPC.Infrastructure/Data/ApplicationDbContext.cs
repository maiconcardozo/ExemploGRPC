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
}
