using SpectrumCare.Identity.Domain.Aggregates.Users;

namespace SpectrumCare.Identity.Domain.Repositories;

/// <summary>
/// Repository interface for User aggregate persistence.
/// Implemented in Infrastructure layer using EF Core.
/// Application layer depends only on this interface — never on EF Core directly.
/// All queries are automatically tenant-scoped via EF Core global query filters.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier.
    /// Returns null if not found or belongs to different tenant.
    /// </summary>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their email address within the current tenant.
    /// Returns null if not found.
    /// </summary>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a user by their refresh token.
    /// Used during JWT refresh token flow.
    /// </summary>
    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an email address is already registered within the tenant.
    /// </summary>
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    void Update(User user);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}