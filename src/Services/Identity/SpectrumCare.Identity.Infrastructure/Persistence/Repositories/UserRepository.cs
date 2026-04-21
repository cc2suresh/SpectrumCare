using Microsoft.EntityFrameworkCore;
using SpectrumCare.Identity.Domain.Aggregates.Users;
using SpectrumCare.Identity.Domain.Repositories;
using SpectrumCare.Identity.Infrastructure.Persistence;

namespace SpectrumCare.Identity.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of IUserRepository.
/// All queries are automatically tenant-scoped via global query filters.
/// Never bypass the DbContext to query users directly.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly IdentityDbContext _context;

    public UserRepository(IdentityDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.Email.Value == email.ToLowerInvariant(),
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<User?> GetByRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                u => u.RefreshToken == refreshToken,
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> EmailExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AnyAsync(
                u => u.Email.Value == email.ToLowerInvariant(),
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    /// <inheritdoc/>
    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    /// <inheritdoc/>
    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}