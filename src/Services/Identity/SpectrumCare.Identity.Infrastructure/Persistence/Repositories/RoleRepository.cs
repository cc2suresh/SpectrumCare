using Microsoft.EntityFrameworkCore;
using SpectrumCare.Identity.Domain.Aggregates.Roles;
using SpectrumCare.Identity.Domain.Repositories;
using SpectrumCare.Identity.Infrastructure.Persistence;

namespace SpectrumCare.Identity.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of IRoleRepository.
/// All queries are automatically tenant-scoped via global query filters.
/// </summary>
public sealed class RoleRepository : IRoleRepository
{
    private readonly IdentityDbContext _context;

    public RoleRepository(IdentityDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<Role?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<Role?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(
                r => r.Name == name,
                cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Role>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Where(r => r.IsActive)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task AddAsync(
        Role role,
        CancellationToken cancellationToken = default)
    {
        await _context.Roles.AddAsync(role, cancellationToken);
    }
}