using SpectrumCare.Identity.Domain.Aggregates.Users;

namespace SpectrumCare.Identity.Domain.Services;

/// <summary>
/// Domain service interface for JWT token generation and validation.
/// Implemented in Infrastructure layer using System.IdentityModel.Tokens.Jwt.
/// Never generate tokens directly in application or domain layers.
/// Tokens include userId, email, roles, and tenantId claims.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT access token for the specified user.
    /// Token includes userId, email, roles, and tenantId claims.
    /// </summary>
    string GenerateAccessToken(User user);

    /// <summary>
    /// Generates a cryptographically secure refresh token.
    /// Used to obtain new access tokens without re-authentication.
    /// </summary>
    string GenerateRefreshToken();

    /// <summary>
    /// Gets the configured access token expiry in minutes.
    /// </summary>
    int AccessTokenExpiryMinutes { get; }

    /// <summary>
    /// Gets the configured refresh token expiry in days.
    /// </summary>
    int RefreshTokenExpiryDays { get; }
}