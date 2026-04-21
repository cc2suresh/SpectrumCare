using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpectrumCare.BuildingBlocks.Web.Responses;
using SpectrumCare.Identity.Application.DTOs;
using SpectrumCare.Identity.Application.Queries.GetUserById;

namespace SpectrumCare.Identity.API.Controllers;

/// <summary>
/// Handles user management endpoints for the Identity service.
/// All endpoints require valid JWT authentication.
/// </summary>
[ApiController]
[Route("api/v1/users")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<UserResponse>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
            return NotFound(ApiResponse<UserResponse>.Failure(
                result.Error.Message,
                new[] { result.Error.Code }));

        return Ok(ApiResponse<UserResponse>.Success(result.Value));
    }
}