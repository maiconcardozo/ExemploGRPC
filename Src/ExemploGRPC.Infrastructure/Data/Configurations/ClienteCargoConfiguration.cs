using ExemploGRPC.Domain.Entities.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExemploGRPC.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for ClienteCargo entity
/// </summary>
public class ClienteCargoConfiguration : IEntityTypeConfiguration<ClienteCargo>
{
    public void Configure(EntityTypeBuilder<ClienteCargo> builder)
    {
        builder.ToTable("ClienteCargos");

        builder.HasKey(cc => cc.Id);

        builder.Property(cc => cc.Id)
            .ValueGeneratedOnAdd();

        builder.Property(cc => cc.ClienteId)
            .IsRequired();

        builder.Property(cc => cc.CargoId)
            .IsRequired();

        builder.Property(cc => cc.DataAtribuicao)
            .IsRequired();

        builder.Property(cc => cc.DataFim);

        builder.Property(cc => cc.IsPrincipal)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(cc => cc.DtCreated)
            .IsRequired();

        builder.Property(cc => cc.DtUpdated);

        builder.Property(cc => cc.DtDeleted);

        builder.Property(cc => cc.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasQueryFilter(cc => cc.IsActive);

        // Create composite unique index to prevent duplicate associations
        builder.HasIndex(cc => new { cc.ClienteId, cc.CargoId })
            .IsUnique();

        // Configure relationships
        builder.HasOne(cc => cc.Cliente)
            .WithMany(c => c.ClienteCargos)
            .HasForeignKey(cc => cc.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Cargo)
            .WithMany(c => c.ClienteCargos)
            .HasForeignKey(cc => cc.CargoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
