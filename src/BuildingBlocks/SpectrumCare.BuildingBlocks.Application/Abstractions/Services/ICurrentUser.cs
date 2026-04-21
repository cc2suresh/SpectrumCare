namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

/// <summary>
/// Provides access to the currently authenticated user's identity and claims.
/// Resolved from JWT token claims on every request.
/// Inject this interface in application layer use cases that need user context.
/// Never inject HttpContext directly into application layer — use this abstraction.
/// </summary>
public interface ICurrentUser
{
    /// <summary>
    /// Gets the unique identifier of the currently authenticated user.
    /// Returns null if the request is unauthenticated.
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Gets the email address of the currently authenticated user.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Gets the full name of the currently authenticated user.
    /// </summary>
    string? FullName { get; }

    /// <summary>
    /// Gets the roles assigned to the currently authenticated user.
    /// </summary>
    IReadOnlyList<string> Roles { get; }

    /// <summary>
    /// Gets the tenant identifier the current user belongs to.
    /// Critical for multi-tenant data isolation.
    /// </summary>
    Guid? TenantId { get; }

    /// <summary>
    /// Determines whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Determines whether the current user has a specific role.
    /// </summary>
    /// <param name="role">The role name to check.</param>
    bool IsInRole(string role);
}