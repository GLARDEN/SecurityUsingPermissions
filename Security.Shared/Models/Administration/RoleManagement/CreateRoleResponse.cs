using Security.Shared.Models.Administration.RoleManagement;

namespace Security.Shared.Models.Administration.Role;

public class CreateRoleResponse
{
    public bool Success { get; set; }
    public RoleDto Role { get; set; }
}

