using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.BuildingBlocks.Domain.ValueObjects;

namespace SpectrumCare.Identity.Domain.ValueObjects;

/// <summary>
/// Represents a validated email address value object.
/// Ensures email format is valid before assignment.
/// Used as the primary identifier for user authentication.
/// </summary>
public sealed class Email : ValueObject
{
    /// <summary>Maximum allowed length for an email address.</summary>
    public const int MaxLength = 256;

    private Email(string value)
    {
        Value = value;
    }

    /// <summary>Gets the email address string value.</summary>
    public string Value { get; }

    /// <summary>
    /// Creates a new Email value object after validation.
    /// Returns failure if email is null, empty, or invalid format.
    /// </summary>
    public static Result<Email> Create(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>(
                new Error("Email.Empty", "Email address cannot be empty."));

        if (email.Length > MaxLength)
            return Result.Failure<Email>(
                new Error("Email.TooLong", $"Email cannot exceed {MaxLength} characters."));

        if (!email.Contains('@') || !email.Contains('.'))
            return Result.Failure<Email>(
                new Error("Email.InvalidFormat", "Email address format is invalid."));

        return Result.Success(new Email(email.ToLowerInvariant().Trim()));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}