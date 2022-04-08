using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions.Enums;

namespace Security.Shared.Models.Administration.Role;

public class DeleteRoleRequest
{
    public const string Route = "api/administration/roleManagement/delete";
    public Guid RoleId { get; set; }
    
}
