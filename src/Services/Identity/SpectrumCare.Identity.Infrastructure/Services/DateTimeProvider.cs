using SpectrumCare.BuildingBlocks.Application.Abstractions.Services;

namespace SpectrumCare.Identity.Infrastructure.Services;

/// <summary>
/// System clock implementation of IDateTimeProvider.
/// Returns real UTC time in production.
/// In tests, mock IDateTimeProvider to return fixed dates.
/// </summary>
public sealed class DateTimeProvider : IDateTimeProvider
{
    /// <inheritdoc/>
    public DateTime UtcNow => DateTime.UtcNow;

    /// <inheritdoc/>
    public DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);

    /// <inheritdoc/>
    public TimeOnly UtcTimeNow => TimeOnly.FromDateTime(DateTime.UtcNow);
}