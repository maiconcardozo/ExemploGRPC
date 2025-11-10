using ExemploGRPC.Domain.Entities.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExemploGRPC.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Cargo entity
/// </summary>
public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
{
    public void Configure(EntityTypeBuilder<Cargo> builder)
    {
        builder.ToTable("Cargos");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Nome)
            .IsUnique();

        builder.Property(c => c.Descricao)
            .HasMaxLength(500);

        builder.Property(c => c.NivelSalarial)
            .HasPrecision(18, 2);

        builder.Property(c => c.DtCreated)
            .IsRequired();

        builder.Property(c => c.DtUpdated);

        builder.Property(c => c.DtDeleted);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasQueryFilter(c => c.IsActive);

        // Configure N-to-N relationship
        builder.HasMany(c => c.ClienteCargos)
            .WithOne(cc => cc.Cargo)
            .HasForeignKey(cc => cc.CargoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
