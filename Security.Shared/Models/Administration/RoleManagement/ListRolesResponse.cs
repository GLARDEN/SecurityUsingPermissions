using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models.Administration.Role;

public class ListRolesResponse
{
    public IEnumerable<RoleDto> Roles { get; set; } = null!;
}

