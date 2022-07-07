using System.Security.Claims;

using Ardalis.Result;

using AutoMapper;

using Security.Core.Models;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Specifications;
using Security.Core.Permissions.Helpers;
using Security.Core.Services;
using Security.SharedKernel.Interfaces;

namespace SecuredAPI.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly IRepository<User> _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IHashService _hashService;

    public AuthenticationService(IMapper mapper,
                                 IRepository<User> userRepository,
                                 IJwtTokenService jwtTokenService,
                                 IRefreshTokenService refreshTokenService,
                                 IHashService hashService)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
        _hashService = hashService;
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        User? user = await _userRepository.GetByIdAsync(new GetUserByIdSpec(changePasswordRequest.UserId));

        if (user == null)
        {
            return new ChangePasswordResponse() { PasswordChangeSuccessfull = false, ErrorMessage = "User Not Found." };
        }

        _hashService.CreateHash(changePasswordRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);
        user.SetPasswordHash(passwordHash, passwordSalt);

        await _userRepository.SaveChangesAsync();

        return new ChangePasswordResponse() { PasswordChangeSuccessfull = true };

    }

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest loginRequest)
    {
        LoginResponse loginResponse = new();

        User? user = await _userRepository.GetBySpecAsync(new GetUserByEmailSpec(loginRequest.Email));

        if (user == null)
        {
            return Result<LoginResponse>.Unauthorized();
        }
        else if (!_hashService.VerifyHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Result<LoginResponse>.Unauthorized();
        }
        else
        {
            var userTokenInfo = new UserTokenDetails()
            {
                Id = user.Id,
                DeviceId = Guid.NewGuid(),
                Email = user.Email,
                Permissions = GetPermissionsForUser(user.UserRoles.ToList() ?? new List<UserRole>())
            };

            RefreshToken refreshToken = _refreshTokenService.GenerateRefreshToken(userTokenInfo);
            user.SetRefreshToken(refreshToken);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            loginResponse.UserId = user.Id;
            loginResponse.JwtToken = _jwtTokenService.GenerateJWTToken(userTokenInfo);
            loginResponse.RefreshToken = refreshToken.Token;
            loginResponse.IsAuthenticationSuccessful = true;

            return Result<LoginResponse>.Success(loginResponse);
        }
    }

    public async Task<Result<LogOutResponse>> LogOutAsync(LogOutRequest logOutRequest)
    {
        
        LogOutResponse loginResponse = new()
        {
            UserId = logOutRequest.UserId
        };

        User? user = await _userRepository.GetBySpecAsync(new GetUserFilterRefreshTokenByDeviceIdSpec(logOutRequest.UserId, logOutRequest.DeviceId));

        if (user != null)
        {
            user.RevokeRefreshToken(logOutRequest.DeviceId);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Result<LogOutResponse>.Success(loginResponse);
        }

        return Result<LogOutResponse>.Success(loginResponse);
    }

    public async Task<RegistrationResponse> RegisterAsync(RegistrationRequestDto registrationRequest, string password)
    {

        if (await UserExists(registrationRequest.Email))
        {
            return new RegistrationResponse()
            {
                IsRegistrationSuccessful = false,
                Errors = new List<string>() { "User already exists." }
            };
        }

        _hashService.CreateHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        User _newUser = new User(Guid.NewGuid(), registrationRequest.Email, passwordHash, passwordSalt);

        await _userRepository.AddAsync(_newUser);

        await _userRepository.SaveChangesAsync();

        RegistrationResponse _registrationResponse = new RegistrationResponse()
        {
            Id = _newUser.Id,
            IsRegistrationSuccessful = true,

        };

        return _registrationResponse;
    }

    public async Task<bool> UserExists(string email)
    {
        if (await _userRepository.AnyAsync(new GetUserByEmailSpec(email)))
        {
            return true;
        }

        return false;
    }

    public async Task<User?> GetUserWithRefreshTokenAsync(Guid id)
    {
        User? user = await _userRepository.GetBySpecAsync(new GetUserWithRefreshTokensSpec(id));
        return user;
    }

    public string GetPermissionsForUser(List<UserRole> userRoles)
    {
        List<string> assignedPermissions = userRoles.Select(ur => ur.AssignedPermissions).ToList();

        if (!assignedPermissions.Any())
            return "";

        //thanks to https://stackoverflow.com/questions/5141863/how-to-get-distinct-characters
        var packedPermissionsForUser = new string(string.Concat(assignedPermissions).Distinct().ToArray());

        return packedPermissionsForUser;
    }

    public async Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        RefreshTokenResponse refreshTokenResponse = new();

        if (!_jwtTokenService.ValidateJwtToken(refreshTokenRequest.JwtToken))
        {
            return Result<RefreshTokenResponse>.Unauthorized();
        }

        ClaimsPrincipal claimsPrincipal = _jwtTokenService.GetPrincipalFromExpiredToken(refreshTokenRequest?.JwtToken);

        Guid userId = claimsPrincipal.GetUserIdFromUser();
        Guid deviceId = claimsPrincipal.GetDeviceIdFromUser();

        User? user = await _userRepository.GetBySpecAsync(new GetUserFilterRefreshTokenByDeviceIdSpec(userId, deviceId));
        if (user != null)
        {
            if (!user.RefreshTokens.Any(r => r.Token.Equals(refreshTokenRequest?.RefreshToken)))
            {
                return Result<RefreshTokenResponse>.Unauthorized();
            }

            RefreshToken? refreshToken = user.RefreshTokens.FirstOrDefault(t => t.Token == refreshTokenRequest?.RefreshToken &&
                                                                                t.DeviceId.Equals(deviceId));

            if (refreshToken != null)
            {
                if (_refreshTokenService.ValidateRefreshToken(refreshTokenRequest?.RefreshToken, refreshToken.TokenHash, refreshToken.TokenSalt))
                {
                    if (!refreshToken.IsValid || refreshToken.Expiry < DateTime.Now)
                    {
                        user.RevokeRefreshToken(refreshToken.DeviceId);
                        await _userRepository.UpdateAsync(user);
                        await _userRepository.SaveChangesAsync();
                        return Result<RefreshTokenResponse>.Unauthorized();
                    }
                    else
                    {
                        var userTokenDetails = new UserTokenDetails()
                        {
                            Id = user.Id,
                            DeviceId = deviceId,
                            Email = user.Email,
                            Permissions = GetPermissionsForUser(user.UserRoles.ToList() ?? new List<UserRole>())
                        };

                        refreshTokenResponse.JWTToken = _jwtTokenService.GenerateJWTToken(userTokenDetails);
                        refreshTokenResponse.RefreshToken = refreshTokenRequest.RefreshToken;
                        refreshTokenResponse.IsAuthenticationSuccessful = true;
                        return Result<RefreshTokenResponse>.Success(refreshTokenResponse);
                    }
                }
            }
            else
            {
                return Result<RefreshTokenResponse>.Unauthorized();
            }
        }
        return Result<RefreshTokenResponse>.Unauthorized();
    }


}
