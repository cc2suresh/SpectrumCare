namespace SpectrumCare.Identity.Infrastructure.Authentication;

/// <summary>
/// Strongly-typed JWT configuration settings.
/// Bound from appsettings.json JwtSettings section.
/// Never hardcode these values — always use configuration.
/// </summary>
public sealed class JwtSettings
{
    /// <summary>Configuration section name in appsettings.json.</summary>
    public const string SectionName = "JwtSettings";

    /// <summary>Gets or sets the secret key for token signing.</summary>
    public string Secret { get; init; } = string.Empty;

    /// <summary>Gets or sets the token issuer.</summary>
    public string Issuer { get; init; } = string.Empty;

    /// <summary>Gets or sets the token audience.</summary>
    public string Audience { get; init; } = string.Empty;

    /// <summary>Gets or sets the access token expiry in minutes.</summary>
    public int ExpiryMinutes { get; init; } = 60;

    /// <summary>Gets or sets the refresh token expiry in days.</summary>
    public int RefreshTokenExpiryDays { get; init; } = 7;
}