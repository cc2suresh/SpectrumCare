using SpectrumCare.Identity.Domain.Services;

namespace SpectrumCare.Identity.Infrastructure.Services;

/// <summary>
/// BCrypt-based implementation of IPasswordHasher.
/// Uses ASP.NET Core PasswordHasher for secure password hashing.
/// Never store plain text passwords — always hash before persisting.
/// Work factor is set to 12 for production-grade security.
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc/>
    public string Hash(string plainTextPassword)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, workFactor: 12);
    }

    /// <inheritdoc/>
    public bool Verify(string plainTextPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
    }
}