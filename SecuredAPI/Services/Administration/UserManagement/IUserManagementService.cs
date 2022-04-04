using Ardalis.Result;

using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;

namespace SecuredAPI.Services;

public interface IUserManagementService
{
    Task<Result<ListUsersResponse>> List();
    Task<EditUserResponse> UpdateUser(EditUserRequest request);
    Task<EditUserRolesResponse> EditUserRoles(EditUserRolesRequest request);
}