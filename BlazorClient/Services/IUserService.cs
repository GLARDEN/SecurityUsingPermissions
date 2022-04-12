using Security.Core.Models;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;

using System.Security.Claims;

namespace BlazorClient.Services;

public interface IUserService
{
    Task<Guid> GetUserIdAsync();
    Task<ClaimsPrincipal> GetCurrentUser();
    Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task<RegistrationResponseDto> RegisterUserAsync(RegistrationRequestDto registrationRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task Logout();
    Task<bool> IsUserAuthenticated();
    Task<List<UserSummaryDto>> ListUsers();
    Task<UpdateUserResponse> Save(UpdateUserRequest updateRequest);
    Task<UpdateUserResponse> UpdateUserAccess(Guid userId, List<UserRoleDto> selectedRoles);
}
