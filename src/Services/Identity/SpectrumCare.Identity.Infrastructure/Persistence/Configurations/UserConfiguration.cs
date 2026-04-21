using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpectrumCare.Identity.Domain.Aggregates.Users;

namespace SpectrumCare.Identity.Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core entity configuration for the User aggregate.
/// Defines table structure, column types, indexes, and value object mappings.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users", "identity");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        // Email value object mapping
        builder.OwnsOne(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName("Email")
                .HasMaxLength(256)
                .IsRequired();

            emailBuilder.HasIndex(e => e.Value)
                .IsUnique();
        });

        // FullName value object mapping
        builder.OwnsOne(u => u.FullName, nameBuilder =>
        {
            nameBuilder.Property(n => n.FirstName)
                .HasColumnName("FirstName")
                .HasMaxLength(100)
                .IsRequired();

            nameBuilder.Property(n => n.LastName)
                .HasColumnName("LastName")
                .HasMaxLength(100)
                .IsRequired();
        });

        // Password value object mapping
        builder.OwnsOne(u => u.Password, passwordBuilder =>
        {
            passwordBuilder.Property(p => p.HashedValue)
                .HasColumnName("PasswordHash")
                .HasMaxLength(512)
                .IsRequired();
        });

        builder.Property(u => u.TenantId)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.IsEmailVerified)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(u => u.FailedLoginAttempts)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(u => u.RefreshToken)
            .HasMaxLength(512);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(256);

        builder.Property(u => u.UpdatedBy)
            .HasMaxLength(256);

        builder.Property(u => u.Version)
            .IsConcurrencyToken();

        builder.HasIndex(u => u.TenantId);
        builder.HasIndex(u => new { u.TenantId, u.IsActive });
    }
}