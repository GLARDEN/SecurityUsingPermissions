
using Security.Core.Models.UserManagement;

namespace BlazorClient.Services;

public interface IUserManagementService
{
    Task<List<UserSummaryDto>> ListUsers();
    Task<UserDto> UpdateUserAccess(UpdateUserRequest updateUserRequest);
}
