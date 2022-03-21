using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Helpers;

namespace Security.Shared.Models;

public class RoleToPermissions
{
    
    public string RoleName { get; set; } = null!;
    public string Description { get; set; } = null!;

    private string _permissionsToRole = null!;
    public IEnumerable<Permission> PermissionsInRole => _permissionsToRole.UnpackPermissionsFromString();

    private RoleToPermissions() { } //required by EF Core

    public RoleToPermissions(string roleName, string description, IEnumerable<Permission> permissions)
    {

        RoleName = roleName;
        Description = description;
    }

    public void UpdatePermissions(IEnumerable<Permission> permissions)
    {
        if (permissions == null || !permissions.Any())
            throw new InvalidOperationException("There should be at least one permission associated with a role.");

        _permissionsToRole = permissions.ConvertPermissionsToDelimitedString();

    }
}