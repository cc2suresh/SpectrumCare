using SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;
using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.Identity.Application.DTOs;
using SpectrumCare.Identity.Domain.Repositories;

namespace SpectrumCare.Identity.Application.Queries.GetUserById;

/// <summary>
/// Handles GetUserByIdQuery.
/// Maps User aggregate to UserResponse DTO.
/// </summary>
public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            query.UserId,
            cancellationToken);

        if (user is null)
            return Result.Failure<UserResponse>(
                Error.NotFound("User", query.UserId));

        return Result.Success(new UserResponse(
            user.Id,
            user.Email.Value,
            user.FullName.FirstName,
            user.FullName.LastName,
            user.FullName.Value,
            user.IsActive,
            user.IsEmailVerified,
            user.LastLoginAt,
            user.Roles.Select(r => r.Name).ToList(),
            user.CreatedAt));
    }
}