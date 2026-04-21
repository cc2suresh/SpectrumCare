using SpectrumCare.BuildingBlocks.Domain.Primitives;
using SpectrumCare.Identity.Domain.Aggregates.Roles;
using SpectrumCare.Identity.Domain.Events;
using SpectrumCare.Identity.Domain.ValueObjects;

namespace SpectrumCare.Identity.Domain.Aggregates.Users;

/// <summary>
/// User aggregate root — the core entity of the Identity domain.
/// Manages user authentication, authorization, and account security.
/// All user state changes must go through this aggregate's methods.
/// Never modify user properties directly — use the provided methods.
/// Raises domain events for all significant state changes.
/// </summary>
public sealed class User : AggregateRoot
{
    /// <summary>Maximum failed login attempts before account lockout.</summary>
    public const int MaxFailedLoginAttempts = 5;

    /// <summary>Account lockout duration in minutes.</summary>
    public const int LockoutDurationMinutes = 30;

    private readonly List<Role> _roles = new();

    private User(
        Guid id,
        Email email,
        FullName fullName,
        Password password,
        Guid tenantId)
        : base(id)
    {
        Email = email;
        FullName = fullName;
        Password = password;
        TenantId = tenantId;
        IsActive = true;
        IsEmailVerified = false;
        FailedLoginAttempts = 0;
    }

    /// <summary>Gets the user's email address.</summary>
    public Email Email { get; private set; }

    /// <summary>Gets the user's full name.</summary>
    public FullName FullName { get; private set; }

    /// <summary>Gets the user's hashed password.</summary>
    public Password Password { get; private set; }

    /// <summary>Gets the tenant this user belongs to.</summary>
    public Guid TenantId { get; private set; }

    /// <summary>Gets whether the user account is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets whether the user's email has been verified.</summary>
    public bool IsEmailVerified { get; private set; }

    /// <summary>Gets the number of consecutive failed login attempts.</summary>
    public int FailedLoginAttempts { get; private set; }

    /// <summary>Gets the datetime until which the account is locked.</summary>
    public DateTime? LockedUntil { get; private set; }

    /// <summary>Gets the last login datetime.</summary>
    public DateTime? LastLoginAt { get; private set; }

    /// <summary>Gets the refresh token for JWT refresh flow.</summary>
    public string? RefreshToken { get; private set; }

    /// <summary>Gets the refresh token expiry datetime.</summary>
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    /// <summary>Gets the roles assigned to this user.</summary>
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();

    /// <summary>Gets whether the account is currently locked.</summary>
    public bool IsLocked => LockedUntil.HasValue && LockedUntil.Value > DateTime.UtcNow;

    /// <summary>
    /// Creates a new User aggregate.
    /// Raises UserRegisteredEvent on successful creation.
    /// </summary>
    public static Result<User> Create(
        Email email,
        FullName fullName,
        Password password,
        Guid tenantId)
    {
        var user = new User(
            Guid.NewGuid(),
            email,
            fullName,
            password,
            tenantId);

        user.RaiseDomainEvent(new UserRegisteredEvent(
            user.Id,
            email.Value,
            fullName.Value,
            tenantId));

        user.IncrementVersion();

        return Result.Success(user);
    }

    /// <summary>
    /// Records a successful login attempt.
    /// Resets failed login counter and updates last login timestamp.
    /// </summary>
    public Result RecordSuccessfulLogin()
    {
        if (!IsActive)
            return Result.Failure(
                new Error("User.Inactive", "User account is inactive."));

        if (IsLocked)
            return Result.Failure(
                new Error("User.Locked", $"Account is locked until {LockedUntil}."));

        FailedLoginAttempts = 0;
        LastLoginAt = DateTime.UtcNow;
        LockedUntil = null;
        IncrementVersion();

        return Result.Success();
    }

    /// <summary>
    /// Records a failed login attempt.
    /// Locks account after MaxFailedLoginAttempts consecutive failures.
    /// Raises UserLockedEvent when account is locked.
    /// </summary>
    public Result RecordFailedLogin()
    {
        if (!IsActive)
            return Result.Failure(
                new Error("User.Inactive", "User account is inactive."));

        FailedLoginAttempts++;

        if (FailedLoginAttempts >= MaxFailedLoginAttempts)
        {
            LockedUntil = DateTime.UtcNow.AddMinutes(LockoutDurationMinutes);

            RaiseDomainEvent(new UserLockedEvent(
                Id,
                Email.Value,
                LockedUntil.Value));
        }

        IncrementVersion();
        return Result.Success();
    }

    /// <summary>
    /// Updates the user's refresh token.
    /// Called after successful JWT token generation.
    /// </summary>
    public void SetRefreshToken(string refreshToken, DateTime expiryTime)
    {
        RefreshToken = refreshToken;
        RefreshTokenExpiryTime = expiryTime;
        IncrementVersion();
    }

    /// <summary>
    /// Changes the user's password.
    /// Raises UserPasswordChangedEvent on success.
    /// </summary>
    public Result ChangePassword(Password newPassword)
    {
        Password = newPassword;

        RaiseDomainEvent(new UserPasswordChangedEvent(Id, Email.Value));
        IncrementVersion();

        return Result.Success();
    }

    /// <summary>
    /// Verifies the user's email address.
    /// </summary>
    public Result VerifyEmail()
    {
        if (IsEmailVerified)
            return Result.Failure(
                new Error("User.AlreadyVerified", "Email is already verified."));

        IsEmailVerified = true;
        IncrementVersion();

        return Result.Success();
    }

    /// <summary>
    /// Assigns a role to the user.
    /// Returns failure if role is already assigned.
    /// </summary>
    public Result AssignRole(Role role)
    {
        if (_roles.Any(r => r.Id == role.Id))
            return Result.Failure(
                new Error("User.RoleAlreadyAssigned",
                    $"Role '{role.Name}' is already assigned to this user."));

        _roles.Add(role);
        IncrementVersion();

        return Result.Success();
    }

    /// <summary>
    /// Deactivates the user account.
    /// </summary>
    public Result Deactivate()
    {
        if (!IsActive)
            return Result.Failure(
                new Error("User.AlreadyInactive", "User account is already inactive."));

        IsActive = false;
        IncrementVersion();

        return Result.Success();
    }

    /// <summary>
    /// Unlocks a locked user account manually.
    /// </summary>
    public Result Unlock()
    {
        LockedUntil = null;
        FailedLoginAttempts = 0;
        IncrementVersion();

        return Result.Success();
    }
}