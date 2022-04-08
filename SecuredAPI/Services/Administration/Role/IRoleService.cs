using Ardalis.Result;

using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;

namespace SecuredAPI.Services;

public interface IRoleService
{
    Task<Result<ListRolesResponse>> ListAsync();
    Task<Result<UpdateRoleResponse>> UpdateAsync(UpdateRoleRequest editRoleRequest);
    Task<Result<CreateRoleResponse>> Create(CreateRoleRequest createRoleRequest);
    Task<Result<DeleteRoleResponse>> Delete(DeleteRoleRequest deleteRoleRequest);
    Task<bool> RoleExists(string roleName);
}