
using Security.Core.Models;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;

using System.Security.Claims;

namespace BlazorClient.Interfaces;

public interface IAuthenticationUiService
{      
    Task<ApiResponse<ChangePasswordResponse>> ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task<ApiResponse<RegistrationResponse>> RegisterUserAsync(RegistrationRequestDto registrationRequestDto);
    Task<ApiResponse<LoginResponse>> Login(LoginRequest loginRequestDto);
    Task<ApiResponse<LogOutResponse>> LogOutAsync(bool logOutAllDevices);
    Task<ApiResponse<UpdateUserResponse>> Save(UpdateUserRequest updateRequest);
    Task<ApiResponse<UpdateUserResponse>> UpdateUserAccess(Guid userId, List<UserRoleDto> selectedRoles);
    Task<GetUserIdResponse> GetUserIdAsync();
    Task<GetUserDeviceIdResponse> GetUserDeviceIdAsync();
    Task<ClaimsPrincipal> GetCurrentUserAsync();
    
}
