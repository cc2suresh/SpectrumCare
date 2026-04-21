using SpectrumCare.BuildingBlocks.Domain.Events;

namespace SpectrumCare.Identity.Domain.Events;

/// <summary>
/// Domain event raised when a user account is locked due to failed login attempts.
/// Consumed by: NotificationService (security alert email).
/// Published via IEventBus after account lockout threshold is reached.
/// </summary>
public sealed class UserLockedEvent : DomainEvent
{
    public UserLockedEvent(
        Guid userId,
        string email,
        DateTime lockedUntil)
    {
        UserId = userId;
        Email = email;
        LockedUntil = lockedUntil;
    }

    /// <summary>Gets the unique identifier of the locked user.</summary>
    public Guid UserId { get; }

    /// <summary>Gets the email address of the locked user.</summary>
    public string Email { get; }

    /// <summary>Gets the datetime until which the account is locked.</summary>
    public DateTime LockedUntil { get; }
}