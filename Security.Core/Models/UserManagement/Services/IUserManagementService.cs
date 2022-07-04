using Ardalis.Result;

using Security.Core.Models.UserManagement;

namespace Security.Core.Models.UserManagement.Services;

public interface IUserManagementService
{
    Task<Result<ListUsersResponse>> ListAsync();
    Task<Result<CreateUserResponse>> CreateAsync(CreateUserRequest createUserRequest);
    Task<Result<UpdateUserResponse>> UpdateAsync(UpdateUserRequest request);
}