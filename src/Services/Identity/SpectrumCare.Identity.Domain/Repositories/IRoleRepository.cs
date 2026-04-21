using SpectrumCare.Identity.Domain.Aggregates.Roles;

namespace SpectrumCare.Identity.Domain.Repositories;

/// <summary>
/// Repository interface for Role entity persistence.
/// Implemented in Infrastructure layer using EF Core.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Gets a role by its unique identifier.
    /// </summary>
    Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a role by its name within the current tenant.
    /// </summary>
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all active roles for the current tenant.
    /// </summary>
    Task<IReadOnlyList<Role>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new role to the repository.
    /// </summary>
    Task AddAsync(Role role, CancellationToken cancellationToken = default);
}