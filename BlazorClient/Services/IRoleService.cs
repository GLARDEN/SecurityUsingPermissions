using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions.Helpers;

namespace BlazorClient.Services;

public interface IRoleService {

    Task<RoleDto> CreateAsync(CreateRoleRequest createRoleRequest);
    Task<RoleDto> UpdateAsync(UpdateRoleRequest createRoleRequest);
    Task DeleteAsync(DeleteRoleRequest deleteRoleRequest);
    Task<List<RoleDto>> ListRoles();
}