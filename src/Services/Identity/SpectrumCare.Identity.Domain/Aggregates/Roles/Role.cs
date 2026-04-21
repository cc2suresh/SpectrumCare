using SpectrumCare.BuildingBlocks.Domain.Primitives;

namespace SpectrumCare.Identity.Domain.Aggregates.Roles;

/// <summary>
/// Represents a role entity in the Identity domain.
/// Roles are assigned to users to control access to platform features.
/// Default system roles: Admin, Manager, Staff, Client.
/// Roles are tenant-scoped — each tenant manages their own roles.
/// </summary>
public sealed class Role : BaseEntity
{
    /// <summary>System-defined role constants.</summary>
    public static class SystemRoles
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Staff = "Staff";
        public const string Client = "Client";
    }

    private Role(Guid id, string name, string? description, Guid tenantId)
        : base(id)
    {
        Name = name;
        Description = description;
        TenantId = tenantId;
        IsActive = true;
    }

    /// <summary>Gets the role name.</summary>
    public string Name { get; private set; }

    /// <summary>Gets the role description.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets the tenant this role belongs to.</summary>
    public Guid TenantId { get; private set; }

    /// <summary>Gets whether this role is active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Creates a new Role entity.
    /// Returns failure if name is null or empty.
    /// </summary>
    public static Result<Role> Create(
        string? name,
        string? description,
        Guid tenantId)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Role>(
                new Error("Role.NameEmpty", "Role name cannot be empty."));

        return Result.Success(new Role(
            Guid.NewGuid(),
            name.Trim(),
            description?.Trim(),
            tenantId));
    }

    /// <summary>Deactivates this role.</summary>
    public void Deactivate() => IsActive = false;

    /// <summary>Activates this role.</summary>
    public void Activate() => IsActive = true;
}