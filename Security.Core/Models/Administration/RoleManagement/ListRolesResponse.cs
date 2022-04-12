
namespace Security.Core.Models.Administration.RoleManagement;

public class ListRolesResponse
{
    public IEnumerable<RoleDto> Roles { get; set; } = null!;
}

