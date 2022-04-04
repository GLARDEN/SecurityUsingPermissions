using Security.Shared.Models;
using Security.Shared.Models.Authentication;
using Security.Shared.Models.UserManagement;

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
    Task<EditUserResponse> Save(EditUserRequest updateRequest);
    Task<EditUserRolesResponse> UpdateUserAccess(Guid userId, List<RoleDisplayDto> selectedRoles);
}
