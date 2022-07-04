using Ardalis.Result;

using Security.Core.Models.Administration.RoleManagement;

namespace Security.Core.Models.Administration.RoleManagement.Services;

public interface IRoleService
{
    Task<Result<ListRolesResponse>> ListAsync();
    Task<Result<UpdateRoleResponse>> UpdateAsync(UpdateRoleRequest editRoleRequest);
    Task<Result<CreateRoleResponse>> CreateAsync(CreateRoleRequest createRoleRequest);
    Task<Result<DeleteRoleResponse>> Delete(DeleteRoleRequest deleteRoleRequest);
    Task<bool> RoleExists(string roleName);
}