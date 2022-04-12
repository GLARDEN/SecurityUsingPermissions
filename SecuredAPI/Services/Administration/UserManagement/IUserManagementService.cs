using Ardalis.Result;
using Security.Core.Models.UserManagement;

namespace SecuredAPI.Services;

public interface IUserManagementService
{
    Task<Result<ListUsersResponse>> ListAsync();
    Task<Result<UpdateUserResponse>> UpdateUserAsync(UpdateUserRequest request);
    //Task<UpdateUserResponse> EditUserRoles(EditUserRolesRequest request);
}