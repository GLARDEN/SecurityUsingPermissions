
using Security.Core.Models;
using Security.Core.Models.Administration.RoleManagement;


namespace BlazorClient.Interfaces;

public interface IRoleUiService
{

    Task<ApiResponse<CreateRoleResponse>> CreateAsync(CreateRoleRequest createRoleRequest);
    Task<ApiResponse<UpdateRoleResponse>> UpdateAsync(UpdateRoleRequest createRoleRequest);
    Task<ApiResponse<DeleteRoleResponse>> DeleteAsync(DeleteRoleRequest deleteRoleRequest);
    Task<ApiResponse<ListRolesResponse>> ListRoles();
}