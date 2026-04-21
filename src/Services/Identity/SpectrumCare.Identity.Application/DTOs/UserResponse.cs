namespace SpectrumCare.Identity.Application.DTOs;

/// <summary>
/// DTO representing a user in API responses.
/// Never expose domain entities directly in API responses.
/// Maps from User aggregate in query handlers.
/// </summary>
public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    bool IsActive,
    bool IsEmailVerified,
    DateTime? LastLoginAt,
    IReadOnlyList<string> Roles,
    DateTime CreatedAt);