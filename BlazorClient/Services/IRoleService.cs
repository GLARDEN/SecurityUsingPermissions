using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions.Helpers;

namespace BlazorClient.Services;

public interface IRoleService {

    Task<CreateRoleResponse> CreateAsync(CreateRoleRequest createRoleRequest);
    Task<DeleteRoleResponse> DeleteAsync(RoleDto role);
    Task<IEnumerable<RoleDto>> ListRoles();
    

}