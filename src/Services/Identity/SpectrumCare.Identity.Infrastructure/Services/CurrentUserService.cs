using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

namespace SpectrumCare.Identity.Infrastructure.Services;

/// <summary>
/// HTTP context-based implementation of ICurrentUser.
/// Resolves user identity from JWT claims on every request.
/// Registered as Scoped — one instance per HTTP request.
/// Never inject IHttpContextAccessor directly in application layer.
/// </summary>
public sealed class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? Principal =>
        _httpContextAccessor.HttpContext?.User;

    /// <inheritdoc/>
    public Guid? UserId
    {
        get
        {
            var value = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    /// <inheritdoc/>
    public string? Email =>
        Principal?.FindFirstValue(ClaimTypes.Email);

    /// <inheritdoc/>
    public string? FullName =>
        Principal?.FindFirstValue(ClaimTypes.Name);

    /// <inheritdoc/>
    public IReadOnlyList<string> Roles =>
        Principal?.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList() ?? new List<string>();

    /// <inheritdoc/>
    public Guid? TenantId
    {
        get
        {
            var value = Principal?.FindFirstValue("tenantId");
            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    /// <inheritdoc/>
    public bool IsAuthenticated =>
        Principal?.Identity?.IsAuthenticated ?? false;

    /// <inheritdoc/>
    public bool IsInRole(string role) =>
        Principal?.IsInRole(role) ?? false;
}