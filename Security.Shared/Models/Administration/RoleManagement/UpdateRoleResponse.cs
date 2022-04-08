using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models.Administration.RoleManagement;

public class UpdateRoleResponse
{

    public RoleDto Role { get; set; } = new RoleDto();

    public UpdateRoleResponse()
    {

    }
}

