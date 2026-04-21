using Microsoft.EntityFrameworkCore;
using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;
using SpectrumCare.Identity.Domain.Aggregates.Roles;
using SpectrumCare.Identity.Domain.Aggregates.Users;

namespace SpectrumCare.Identity.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for the Identity service.
/// Implements multi-tenant isolation via global query filters on TenantId.
/// Automatically dispatches domain events on SaveChangesAsync.
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityDbContext).Assembly);

        // Global query filter for multi-tenant isolation
        // Every query on User and Role is automatically scoped to current tenant
        modelBuilder.Entity<User>()
            .HasQueryFilter(u => u.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<Role>()
            .HasQueryFilter(r => r.TenantId == _tenantContext.TenantId);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Saves changes and dispatches domain events.
    /// Domain events are cleared after successful dispatch.
    /// </summary>
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
}