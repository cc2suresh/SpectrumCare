using SpectrumCare.BuildingBlocks.Application.Abstractions.Messaging;
using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.Identity.Domain.Aggregates.Users;
using SpectrumCare.Identity.Domain.Repositories;
using SpectrumCare.Identity.Domain.Services;
using SpectrumCare.Identity.Domain.ValueObjects;

namespace SpectrumCare.Identity.Application.Commands.Register;

/// <summary>
/// Handles user registration command.
/// Validates uniqueness, hashes password, creates user aggregate.
/// Persists user and dispatches UserRegisteredEvent via domain events.
/// </summary>
public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<Guid>> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken)
    {
        // Validate email
        var emailResult = Email.Create(command.Email);
        if (emailResult.IsFailure)
            return Result.Failure<Guid>(emailResult.Error);

        // Validate full name
        var fullNameResult = FullName.Create(command.FirstName, command.LastName);
        if (fullNameResult.IsFailure)
            return Result.Failure<Guid>(fullNameResult.Error);

        // Validate password complexity
        var complexityResult = Password.ValidateComplexity(command.Password);
        if (complexityResult.IsFailure)
            return Result.Failure<Guid>(complexityResult.Error);

        // Check email uniqueness
        var emailExists = await _userRepository.EmailExistsAsync(
            emailResult.Value.Value,
            cancellationToken);

        if (emailExists)
            return Result.Failure<Guid>(
                new Error("User.EmailExists",
                    "A user with this email already exists."));

        // Hash password
        var hashedPassword = _passwordHasher.Hash(command.Password);
        var passwordResult = Password.Create(hashedPassword);
        if (passwordResult.IsFailure)
            return Result.Failure<Guid>(passwordResult.Error);

        // Create user aggregate
        var userResult = User.Create(
            emailResult.Value,
            fullNameResult.Value,
            passwordResult.Value,
            command.TenantId);

        if (userResult.IsFailure)
            return Result.Failure<Guid>(userResult.Error);

        var user = userResult.Value;

        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}