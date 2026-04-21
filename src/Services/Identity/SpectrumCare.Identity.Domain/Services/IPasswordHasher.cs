namespace SpectrumCare.Identity.Domain.Services;

/// <summary>
/// Domain service interface for password hashing and verification.
/// Implemented in Infrastructure layer using BCrypt or ASP.NET Core PasswordHasher.
/// Never implement password hashing in domain or application layers.
/// This abstraction keeps the domain free of infrastructure concerns.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes a plain text password.
    /// Always call Password.ValidateComplexity() before calling this method.
    /// </summary>
    /// <param name="plainTextPassword">The plain text password to hash.</param>
    /// <returns>The hashed password string.</returns>
    string Hash(string plainTextPassword);

    /// <summary>
    /// Verifies a plain text password against a hashed password.
    /// Returns true if the password matches the hash.
    /// </summary>
    /// <param name="plainTextPassword">The plain text password to verify.</param>
    /// <param name="hashedPassword">The hashed password to verify against.</param>
    bool Verify(string plainTextPassword, string hashedPassword);
}