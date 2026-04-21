using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.BuildingBlocks.Domain.ValueObjects;

namespace SpectrumCare.Identity.Domain.ValueObjects;

/// <summary>
/// Represents a hashed password value object.
/// Never stores plain text passwords.
/// Password hashing is handled by IPasswordHasher in Infrastructure layer.
/// This value object only stores and validates the hashed value.
/// </summary>
public sealed class Password : ValueObject
{
    /// <summary>Minimum required password length.</summary>
    public const int MinLength = 8;

    /// <summary>Maximum allowed password length.</summary>
    public const int MaxLength = 256;

    private Password(string hashedValue)
    {
        HashedValue = hashedValue;
    }

    /// <summary>Gets the hashed password value.</summary>
    public string HashedValue { get; }

    /// <summary>
    /// Creates a Password value object from an already-hashed value.
    /// Call this after hashing with IPasswordHasher.
    /// Never pass plain text passwords to this method.
    /// </summary>
    public static Result<Password> Create(string? hashedValue)
    {
        if (string.IsNullOrWhiteSpace(hashedValue))
            return Result.Failure<Password>(
                new Error("Password.Empty", "Password cannot be empty."));

        return Result.Success(new Password(hashedValue));
    }

    /// <summary>
    /// Validates plain text password meets complexity requirements.
    /// Call this BEFORE hashing to enforce password policy.
    /// </summary>
    public static Result ValidateComplexity(string? plainTextPassword)
    {
        if (string.IsNullOrWhiteSpace(plainTextPassword))
            return Result.Failure(
                new Error("Password.Empty", "Password cannot be empty."));

        if (plainTextPassword.Length < MinLength)
            return Result.Failure(
                new Error("Password.TooShort",
                    $"Password must be at least {MinLength} characters."));

        if (plainTextPassword.Length > MaxLength)
            return Result.Failure(
                new Error("Password.TooLong",
                    $"Password cannot exceed {MaxLength} characters."));

        if (!plainTextPassword.Any(char.IsUpper))
            return Result.Failure(
                new Error("Password.NoUppercase",
                    "Password must contain at least one uppercase letter."));

        if (!plainTextPassword.Any(char.IsLower))
            return Result.Failure(
                new Error("Password.NoLowercase",
                    "Password must contain at least one lowercase letter."));

        if (!plainTextPassword.Any(char.IsDigit))
            return Result.Failure(
                new Error("Password.NoDigit",
                    "Password must contain at least one digit."));

        if (!plainTextPassword.Any(ch => !char.IsLetterOrDigit(ch)))
            return Result.Failure(
                new Error("Password.NoSpecialChar",
                    "Password must contain at least one special character."));

        return Result.Success();
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return HashedValue;
    }
}