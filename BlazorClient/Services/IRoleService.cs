using Security.Core.Models.Administration.RoleManagement;


namespace BlazorClient.Services;

public interface IRoleService {

    Task<RoleDto> CreateAsync(CreateRoleRequest createRoleRequest);
    Task<RoleDto> UpdateAsync(UpdateRoleRequest createRoleRequest);
    Task DeleteAsync(DeleteRoleRequest deleteRoleRequest);
    Task<List<RoleDto>> ListRoles();
}