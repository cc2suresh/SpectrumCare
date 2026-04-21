namespace SpectrumCare.Identity.API.Contracts.Requests;

/// <summary>
/// HTTP request contract for user login.
/// </summary>
public sealed record LoginRequest(
    string Email,
    string Password,
    Guid TenantId);