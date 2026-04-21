namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

/// <summary>
/// Provides access to the current tenant context for multi-tenant isolation.
/// Every database query must be scoped to the current tenant via this interface.
/// EF Core global query filters use this to automatically filter all queries.
/// Never bypass this interface — doing so risks cross-tenant data leakage.
/// </summary>
public interface ITenantContext
{
    /// <summary>
    /// Gets the unique identifier of the current tenant.
    /// Resolved from JWT claims on every request.
    /// Throws InvalidOperationException if accessed outside a tenant context.
    /// </summary>
    Guid TenantId { get; }

    /// <summary>
    /// Gets the name of the current tenant organization.
    /// </summary>
    string? TenantName { get; }

    /// <summary>
    /// Determines whether a valid tenant context exists for the current request.
    /// Always check this before accessing TenantId in background jobs.
    /// </summary>
    bool HasTenant { get; }
}