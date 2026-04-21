using SpectrumCare.BuildingBlocks.Domain.Events;

namespace SpectrumCare.Identity.Domain.Events;

/// <summary>
/// Domain event raised when a user changes their password.
/// Consumed by: NotificationService (password change confirmation email).
/// </summary>
public sealed class UserPasswordChangedEvent : DomainEvent
{
    public UserPasswordChangedEvent(Guid userId, string email)
    {
        UserId = userId;
        Email = email;
    }

    /// <summary>Gets the unique identifier of the user.</summary>
    public Guid UserId { get; }

    /// <summary>Gets the email address of the user.</summary>
    public string Email { get; }
}