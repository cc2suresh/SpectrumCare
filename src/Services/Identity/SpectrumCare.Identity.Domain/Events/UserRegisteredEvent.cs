using SpectrumCare.BuildingBlocks.Domain.Events;

namespace SpectrumCare.Identity.Domain.Events;

/// <summary>
/// Domain event raised when a new user successfully registers.
/// Consumed by: NotificationService (welcome email), ClientHub (profile creation).
/// Published via IEventBus after successful user creation.
/// </summary>
public sealed class UserRegisteredEvent : DomainEvent
{
    public UserRegisteredEvent(
        Guid userId,
        string email,
        string fullName,
        Guid tenantId)
    {
        UserId = userId;
        Email = email;
        FullName = fullName;
        TenantId = tenantId;
    }

    /// <summary>Gets the unique identifier of the registered user.</summary>
    public Guid UserId { get; }

    /// <summary>Gets the email address of the registered user.</summary>
    public string Email { get; }

    /// <summary>Gets the full name of the registered user.</summary>
    public string FullName { get; }

    /// <summary>Gets the tenant the user belongs to.</summary>
    public Guid TenantId { get; }
}