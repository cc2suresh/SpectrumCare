using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

namespace SpectrumCare.Identity.Infrastructure.Services;

/// <summary>
/// HTTP context-based implementation of ITenantContext.
/// Resolves tenant identity from JWT claims on every request.
/// Returns empty Guid for unauthenticated requests on public endpoints.
/// Registered as Scoped — one instance per HTTP request.
/// </summary>
public sealed class TenantContextService : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private string? GetTenantIdClaim() =>
        _httpContextAccessor.HttpContext?
            .User.FindFirstValue("tenantId");

    /// <inheritdoc/>
    public Guid TenantId
    {
        get
        {
            var value = GetTenantIdClaim();
            return Guid.TryParse(value, out var tenantId)
                ? tenantId
                : Guid.Empty;
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
            var value = GetTenantIdClaim();
            return Guid.TryParse(value, out var id) && id != Guid.Empty;
        }
    }
}