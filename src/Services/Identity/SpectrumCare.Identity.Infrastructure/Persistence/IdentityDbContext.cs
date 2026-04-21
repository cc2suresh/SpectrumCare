using Microsoft.EntityFrameworkCore;
using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;
using SpectrumCare.Identity.Domain.Aggregates.Roles;
using SpectrumCare.Identity.Domain.Aggregates.Users;

namespace SpectrumCare.Identity.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for the Identity service.
/// Implements multi-tenant isolation via global query filters on TenantId.
/// Gracefully handles unauthenticated requests for public endpoints.
/// Never share this DbContext with other services.
/// </summary>
public sealed class IdentityDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    /// <summary>Gets the Users table.</summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>Gets the Roles table.</summary>
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IdentityDbContext).Assembly);

        // Global query filter for multi-tenant isolation
        // HasTenant check prevents filter from breaking public endpoints
        modelBuilder.Entity<User>()
            .HasQueryFilter(u =>
                !_tenantContext.HasTenant ||
                u.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Role>()
            .HasQueryFilter(r =>
                !_tenantContext.HasTenant ||
                r.TenantId == _tenantContext.TenantId);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves changes to the database.
    /// </summary>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}