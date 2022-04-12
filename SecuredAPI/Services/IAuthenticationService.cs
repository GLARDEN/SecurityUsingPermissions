using Security.Core.Models;
using Security.Core.Models.Authentication;

namespace SecuredAPI.Services;

public interface IAuthenticationService {
    Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    Task<RegistrationResponseDto> RegisterAsync(RegistrationRequestDto registrationRequest, string password);
    Task<bool> UserExists(string email);
    
}
