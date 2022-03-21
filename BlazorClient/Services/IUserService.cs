using Security.Shared.Models;
using Security.Shared.Models.Authentication;

using System.Security.Claims;

namespace BlazorClient.Services;

public interface IUserService
{
    Task<ClaimsPrincipal> GetCurrentUser();
    Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task<RegistrationResponseDto> RegisterUserAsync(RegistrationRequestDto registrationRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    Task Logout();
    Task<bool> IsUserAuthenticated();
    
}
