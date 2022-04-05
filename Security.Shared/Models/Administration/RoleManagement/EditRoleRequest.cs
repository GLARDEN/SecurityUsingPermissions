using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models.Administration.RoleManagement;

public class EditRoleRequest
{
    public const string Route = "api/administration/rolemanagement/edit";
    public string RoleName { get; set; } = null!;
    public List<string> PermissionNames { get; set; } = null!;
}

