
namespace Security.Core.Models.Administration.RoleManagement;

public class DeleteRoleRequest
{
    public const string Route = "api/administration/roleManagement/delete";
    public Guid RoleId { get; set; }

}
