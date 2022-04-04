using Security.Shared.Models.Administration.Role;

namespace SecuredAPI.Services;

public interface IRoleService
{
    Task<ListRolesResponse> GetRolesAsync();
    Task<CreateRoleResponse> Create(CreateRoleRequest createRoleRequest);
    Task<DeleteRoleResponse> Delete(DeleteRoleRequest deleteRoleRequest);
    Task<bool> RoleExists(string roleName);
}