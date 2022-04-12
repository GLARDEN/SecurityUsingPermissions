using Ardalis.GuardClauses;

using Security.Core.Models.Administration.RoleManagement.Events;
using Security.Core.Permissions.Extensions;

using Security.SharedKernel.Interfaces;
using Security.Core.Events;

namespace Security.Core.Models.Administration.RoleManagement;

public class Role : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;

    public string PermissionsInRole { get; private set; }
    public bool Enabled { get; private set; }

    private Role() { } //required by EF Core

    public Role(string roleName, string description, bool enabled, string permissions)
    {
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "A role should have atleaast 1 permisssion assigned");
        Name = roleName;
        Description = description;
        Enabled = enabled;
        PermissionsInRole = permissions;
    }

    public static Role Create(string roleName, string description, bool enabled, string permissions)
    {
        Guard.Against.NullOrEmpty(roleName, nameof(roleName), "Role name is required.");



        Role newRole = new Role(roleName, description, enabled, permissions);
        return newRole;

    }

    public void ChangeRoleDescription(string? description)
    {
        if (!Description.Equals(description))
        {
            Description = description;
        }

    }

    public async Task RenameRoleAsync(string roleName)
    {
        Guard.Against.Null(roleName, nameof(roleName), "Role name requred.");

        await DomainEvents.Raise(new UpdatingRoleNameEvent(Id, roleName));

        if (!Name.Equals(roleName))
        {
            Name = roleName;
        }
    }

    public void UpdatePermissions(IEnumerable<string> permissions)
    {
        Guard.Against.Null(permissions, nameof(permissions), "A role should have atleaast 1 permisssion assigned");
        PermissionsInRole = permissions.PackPermissionsNames();
    }

    public void ToggleRoleEnabled()
    {
        Enabled = !Enabled;
    }
}
