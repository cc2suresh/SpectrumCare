namespace SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

/// <summary>
/// Abstraction for date and time operations.
/// Never use DateTime.UtcNow directly in application or domain code.
/// Always inject this interface instead — it makes time-dependent logic fully testable.
/// In tests, mock this to return fixed dates for deterministic results.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// Use this for all timestamp operations across the platform.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current UTC date without time component.
    /// Use this for date-only comparisons like appointment dates.
    /// </summary>
    DateOnly UtcToday { get; }

    /// <summary>
    /// Gets the current UTC time without date component.
    /// Use this for time-only comparisons like schedule slots.
    /// </summary>
    TimeOnly UtcTimeNow { get; }
}