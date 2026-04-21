namespace SpectrumCare.Identity.API.Contracts.Requests;

/// <summary>
/// HTTP request contract for user registration.
/// Validated by RegisterRequestValidator before reaching the handler.
/// </summary>
public sealed record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    Guid TenantId);