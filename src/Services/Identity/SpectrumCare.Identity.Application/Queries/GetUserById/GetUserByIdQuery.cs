using SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;
using SpectrumCare.Identity.Application.DTOs;

namespace SpectrumCare.Identity.Application.Queries.GetUserById;

/// <summary>
/// Query to retrieve a user by their unique identifier.
/// Returns UserResponse DTO — never returns domain entity directly.
/// </summary>
public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;