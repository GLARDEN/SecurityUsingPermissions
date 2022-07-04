
using Security.Core.Models;
using Security.Core.Models.UserManagement;

namespace BlazorClient.Interfaces;

public interface IUserManagementUiService
{
    Task<ApiResponse<ListUsersResponse>> ListUsers();
    Task<ApiResponse<CreateUserResponse>> CreateAsync(CreateUserRequest createUserRequest);
    Task<ApiResponse<UpdateUserResponse>> UpdateAsnyc(UpdateUserRequest updateUserRequest);
    
}
