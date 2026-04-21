using SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;

namespace SpectrumCare.Identity.Application.Commands.Register;

/// <summary>
/// Command to register a new user in the system.
/// Returns the newly created user's unique identifier on success.
/// Raises UserRegisteredEvent after successful registration.
/// </summary>
public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Guid TenantId) : ICommand<Guid>;