using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

namespace SpectrumCare.Identity.Infrastructure.Services;

/// <summary>
/// HTTP context-based implementation of ITenantContext.
/// Resolves tenant identity from JWT claims on every request.
/// Critical for multi-tenant data isolation via EF Core global query filters.
/// Registered as Scoped — one instance per HTTP request.
/// </summary>
public sealed class TenantContextService : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc/>
    public Guid TenantId
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User.FindFirstValue("tenantId");

            if (!Guid.TryParse(value, out var tenantId))
                throw new InvalidOperationException(
                    "TenantId claim is missing or invalid. Ensure JWT contains tenantId claim.");

            return tenantId;
        }
    }

    /// <inheritdoc/>
    public string? TenantName =>
        _httpContextAccessor.HttpContext?
            .User.FindFirstValue("tenantName");

    /// <inheritdoc/>
    public bool HasTenant
    {
        get
        {
            var value = _httpContextAccessor.HttpContext?
                .User.FindFirstValue("tenantId");
            return Guid.TryParse(value, out _);
        }
    }
}