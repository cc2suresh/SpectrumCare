using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpectrumCare.BuildingBlocks.Web.Responses;
using SpectrumCare.Identity.API.Contracts.Requests;
using SpectrumCare.Identity.Application.Commands.Login;
using SpectrumCare.Identity.Application.Commands.Register;

namespace SpectrumCare.Identity.API.Controllers;

/// <summary>
/// Handles authentication endpoints for the Identity service.
/// All endpoints are versioned under /api/v1/auth.
/// Register and Login are public — no JWT required.
/// All other endpoints require valid JWT.
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Registers a new user account.
    /// Returns the newly created user ID on success.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.TenantId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(ApiResponse<Guid>.Failure(
                result.Error.Message,
                new[] { result.Error.Code }));

        return CreatedAtAction(
            nameof(Register),
            ApiResponse<Guid>.Success(result.Value, "User registered successfully."));
    }

    /// <summary>
    /// Authenticates a user and returns JWT tokens.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password,
            request.TenantId);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return Unauthorized(ApiResponse<object>.Failure(
                result.Error.Message,
                new[] { result.Error.Code }));

        return Ok(ApiResponse<object>.Success(result.Value, "Login successful."));
    }
}