using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.BuildingBlocks.Domain.ValueObjects;

namespace SpectrumCare.Identity.Domain.ValueObjects;

/// <summary>
/// Represents a validated full name value object.
/// Combines first and last name into a single immutable value.
/// Used across Identity and other services for display purposes.
/// </summary>
public sealed class FullName : ValueObject
{
    /// <summary>Maximum allowed length for first or last name.</summary>
    public const int MaxLength = 100;

    private FullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    /// <summary>Gets the first name.</summary>
    public string FirstName { get; }

    /// <summary>Gets the last name.</summary>
    public string LastName { get; }

    /// <summary>Gets the full name as a single string.</summary>
    public string Value => $"{FirstName} {LastName}";

    /// <summary>
    /// Creates a new FullName value object after validation.
    /// Returns failure if either name is null, empty, or exceeds max length.
    /// </summary>
    public static Result<FullName> Create(string? firstName, string? lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure<FullName>(
                new Error("FullName.FirstNameEmpty", "First name cannot be empty."));

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure<FullName>(
                new Error("FullName.LastNameEmpty", "Last name cannot be empty."));

        if (firstName.Length > MaxLength)
            return Result.Failure<FullName>(
                new Error("FullName.FirstNameTooLong", $"First name cannot exceed {MaxLength} characters."));

        if (lastName.Length > MaxLength)
            return Result.Failure<FullName>(
                new Error("FullName.LastNameTooLong", $"Last name cannot exceed {MaxLength} characters."));

        return Result.Success(new FullName(
            firstName.Trim(),
            lastName.Trim()));
    }

    /// <inheritdoc/>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return FirstName;
        yield return LastName;
    }

    /// <inheritdoc/>
    public override string ToString() => Value;
}