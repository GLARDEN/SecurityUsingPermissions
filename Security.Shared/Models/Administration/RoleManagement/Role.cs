using Ardalis.GuardClauses;

using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Extensions;
using Security.Shared.Permissions.Helpers;

namespace Security.Shared.Models.Administration.RoleManagement;

public class Role
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
    public void ChangeRoleDescription(string? description)
    {
        if (!Description.Equals(description))
        {
            Description = description;
        }
        
    }

    public void RenameRole(string roleName)
    {
        Guard.Against.Null(roleName, nameof(roleName), "Role name requred.");

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
