using Ardalis.Result;

using Security.Core.Models;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;

namespace SecuredAPI.Services;

public interface IAuthenticationService
{
    Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequestDto);
    Task<Result<LogOutResponse>> LogOutAsync(LogOutRequest logOutRequest);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequestDto registrationRequest, string password);
    Task<bool> UserExists(string email);
    Task<User?> GetUserWithRefreshTokenAsync(Guid id);
    string GetPermissionsForUser(List<UserRole> userRoles);

    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);

}
