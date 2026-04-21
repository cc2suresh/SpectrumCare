using SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;
using SpectrumCare.Identity.Application.DTOs;

namespace SpectrumCare.Identity.Application.Commands.Login;

/// <summary>
/// Command to authenticate a user and return JWT tokens.
/// Returns access token and refresh token on success.
/// Records failed login attempts and locks account if threshold exceeded.
/// </summary>
public sealed record LoginCommand(
    string Email,
    string Password,
    Guid TenantId) : ICommand<AuthTokenResponse>;