using ExemploGRPC.Domain.Entities.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExemploGRPC.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Cliente entity
/// </summary>
public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> builder)
    {
        builder.ToTable("Clientes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Nome)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(c => c.Cpf)
            .IsUnique();

        builder.Property(c => c.Telefone)
            .HasMaxLength(20);

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
            .WithOne(cc => cc.Cliente)
            .HasForeignKey(cc => cc.ClienteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
