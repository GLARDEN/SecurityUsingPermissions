using Ardalis.GuardClauses;

using Security.Core.Models.Administration.RoleManagement.Events;
using Security.Core.Permissions.Extensions;

using Security.SharedKernel.Interfaces;
using Security.SharedKernel;
using Security.Core.Models.UserManagement.Events;

namespace Security.Core.Models.Administration.RoleManagement;

public class Role : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    public string PermissionsInRole { get; private set; }
    public bool Enabled { get; private set; }

    private Role() { } //required by EF Core

    private Role(string roleName, string description, bool enabled, string permissions)
    {
        Name = Guard.Against.NullOrEmpty(roleName, nameof(roleName));
        Description = description;
        Enabled = Guard.Against.Null(enabled, nameof(enabled));
        PermissionsInRole = Guard.Against.NullOrEmpty(permissions, nameof(permissions));
    }
    public static Role Create(string roleName, string? description, bool enabled, string permissions)
    {
        Guard.Against.NullOrEmpty(roleName, nameof(roleName), "Role must have a name.");
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "A role should have atleaast 1 permisssion assigned");

        Role newRole = new();
        newRole.RenameRole(roleName);
        newRole.ChangeRoleDescription(description);
        newRole.UpdatePermissions(permissions);
        newRole.UpdateEnabled(enabled);
        return newRole;
    }
    public void ChangeRoleDescription(string? description)
    {
        if (Description == null || !Description.Equals(description))
        {
            Description = description ?? "";
        }
    }

    public void RenameRole(string roleName)
    {
        Guard.Against.Null(roleName, nameof(roleName), "Role name requred.");
        DomainEvents.Raise(new UpdatingRoleNameEvent(this.Id, roleName)).Wait();

        if (Name == null || !Name.Equals(roleName))
        {
            Name = roleName;
        }
    }

    public void UpdatePermissions(string permissions)
    {
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "A role should have atleaast 1 permisssion assigned");
        DomainEvents.Raise(new CheckForInvalidPermissionEvent(permissions)).Wait();
        PermissionsInRole = permissions;
    }

    public void UpdateEnabled(bool enabled)
    {
        Enabled = enabled;
    }
}
