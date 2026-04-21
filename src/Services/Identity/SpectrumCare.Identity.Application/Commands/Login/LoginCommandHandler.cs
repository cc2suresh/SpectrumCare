using SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;
using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.Identity.Application.DTOs;
using SpectrumCare.Identity.Domain.Repositories;
using SpectrumCare.Identity.Domain.Services;

namespace SpectrumCare.Identity.Application.Commands.Login;

/// <summary>
/// Handles user login command.
/// Validates credentials, records login attempts, generates JWT tokens.
/// Returns AuthTokenResponse on success.
/// </summary>
public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthTokenResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<Result<AuthTokenResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        // Get user by email
        var user = await _userRepository.GetByEmailAsync(
            command.Email,
            cancellationToken);

        if (user is null)
            return Result.Failure<AuthTokenResponse>(
                new Error("Auth.InvalidCredentials",
                    "Invalid email or password."));

        // Check account lock
        if (user.IsLocked)
            return Result.Failure<AuthTokenResponse>(
                new Error("Auth.AccountLocked",
                    $"Account is locked until {user.LockedUntil}."));

        // Verify password
        var isPasswordValid = _passwordHasher.Verify(
            command.Password,
            user.Password.HashedValue);

        if (!isPasswordValid)
        {
            user.RecordFailedLogin();
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return Result.Failure<AuthTokenResponse>(
                new Error("Auth.InvalidCredentials",
                    "Invalid email or password."));
        }

        // Generate tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow
            .AddDays(_jwtTokenService.RefreshTokenExpiryDays);

        // Update user
        user.SetRefreshToken(refreshToken, refreshTokenExpiry);
        user.RecordSuccessfulLogin();
        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthTokenResponse(
            accessToken,
            refreshToken,
            DateTime.UtcNow.AddMinutes(_jwtTokenService.AccessTokenExpiryMinutes),
            refreshTokenExpiry,
            user.Id,
            user.Email.Value,
            user.FullName.Value,
            user.Roles.Select(r => r.Name).ToList()));
    }
}