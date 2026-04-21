using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpectrumCare.Identity.Domain.Aggregates.Roles;

namespace SpectrumCare.Identity.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core entity configuration for the Role entity.
/// </summary>
public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles", "identity");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever();

        builder.Property(r => r.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(r => r.Description)
            .HasMaxLength(500);

        builder.Property(r => r.TenantId)
            .IsRequired();

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasIndex(r => new { r.TenantId, r.Name })
            .IsUnique();
    }
}