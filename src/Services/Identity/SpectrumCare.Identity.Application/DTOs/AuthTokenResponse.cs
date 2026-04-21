namespace SpectrumCare.Identity.Application.DTOs;

/// <summary>
/// Response DTO returned after successful authentication.
/// Contains JWT access token, refresh token, and expiry information.
/// Never include sensitive user data beyond what is listed here.
/// </summary>
public sealed record AuthTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiry,
    DateTime RefreshTokenExpiry,
    Guid UserId,
    string Email,
    string FullName,
    IReadOnlyList<string> Roles);