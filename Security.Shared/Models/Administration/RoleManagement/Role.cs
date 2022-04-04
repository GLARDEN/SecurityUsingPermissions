using Ardalis.GuardClauses;

using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Helpers;

namespace Security.Shared.Models.Administration.RoleManagement;

public class Role
{
    public string RoleName { get; set; } = null!;
    public string Description { get; set; } = null!;    
    public string PermissionsInRole { get; set; }
    public bool Enabled { get; set; }

    private Role() { } //required by EF Core

    public Role(string roleName, string description, bool enabled,string permissions)
    {
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "A role should have atleaast 1 permisssion assigned");

        RoleName = roleName;
        Description = description;
        Enabled = enabled;        
        UpdatePermissions(permissions);
    }

    public void UpdatePermissions(string permissions)
    {
        Guard.Against.NullOrEmpty(permissions, nameof(permissions), "A role should have atleaast 1 permisssion assigned");
        PermissionsInRole = permissions;
    }
}
