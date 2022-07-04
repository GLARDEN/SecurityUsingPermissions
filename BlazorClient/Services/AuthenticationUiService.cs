using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Security.Core.Models;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;
using System.Net;
using BlazorClient.Interfaces;
using Security.Core.Models.Administration.RoleManagement;
using BlazorClient.Providers;
using System.Net.Http.Headers;
using System.Text;
using System.Linq;
using Security.Core.Permissions.Constants;
using Security.Core.Permissions.Helpers;
using Ardalis.Result;

namespace BlazorClient.Services;

public class AuthenticationUiService : IAuthenticationUiService
{
    private readonly IHttpService _httpService;

    private readonly JsonSerializerOptions _options;

    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenUiService _refreshTokenService;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthenticationUiService(IHttpService httpService, IJwtTokenService jwtTokenService, IRefreshTokenUiService refreshTokenService, AuthenticationStateProvider authStateProvider)
    {
        _httpService = httpService;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
        _authStateProvider = authStateProvider;
    }

    public async Task<ApiResponse<ChangePasswordResponse>> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        ApiResponse<ChangePasswordResponse> apiResponse = await _httpService.HttpPostAsync<ChangePasswordResponse>("authentication/changepassword", changePasswordRequest);
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            return apiResponse;
        }
        else
        {
            //handle login validation errors or exceptions
            return apiResponse;
        }
    }

    public async Task<ApiResponse<LoginResponse>> Login(LoginRequest loginRequestDto)
    {

        ApiResponse<LoginResponse> apiResponse = await _httpService.HttpPostAsync<LoginResponse>(LoginRequest.Route, loginRequestDto);
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            await _jwtTokenService.SetJwtTokenAsync(apiResponse.Data?.JwtToken ?? "");
            await _refreshTokenService.SetRefreshTokenAsync(apiResponse.Data?.RefreshToken ?? "");
            await ((AuthStateProvider)_authStateProvider).NotifyUserAuthenticationAsync();
            apiResponse.Data.IsAuthenticationSuccessful = true;
            return apiResponse;
        }
        else
        {
            //Process validation messages or exceptions
            return apiResponse;
        }
    }


    public async Task<ApiResponse<LogOutResponse>> LogOutAsync(bool logOutAllDevices)
    {
        Guid deviceId = Guid.Empty;


        string jwtToken = await _jwtTokenService.GetJwtTokenAsync();
        IEnumerable<Claim> claims =  _jwtTokenService.GetClaimsFromJwtToken(jwtToken);

        if (!logOutAllDevices)
        {
            deviceId = claims.GetDeviceIdFromClaims();
        }

        LogOutRequest logOutRequest = new()
        {
            UserId = claims.GetUserIdFromClaims(),
            DeviceId = deviceId
        };

        ApiResponse<LogOutResponse> apiResponse = await _httpService.HttpPostAsync<LogOutResponse>(LogOutRequest.Route, logOutRequest);
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            await _jwtTokenService.RemoveJwtTokenAsync();
            await _refreshTokenService.RemoveRefreshTokenAsync();
        }
                
        ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

        return apiResponse;
    }

    public async Task<GetUserIdResponse> GetUserIdAsync()
    {
        GetUserIdResponse getUserIdResponse = new()
        {
            UserId = (await _authStateProvider.GetAuthenticationStateAsync()).User.Claims.GetUserIdFromClaims()
        };

        return getUserIdResponse;
    }

    public async Task<GetUserDeviceIdResponse> GetUserDeviceIdAsync()
    {
        GetUserDeviceIdResponse getUserDeviceIdResponse = new()
        {
            DeviceId = (await _authStateProvider.GetAuthenticationStateAsync()).User.Claims.GetDeviceIdFromClaims() 
        };

        return getUserDeviceIdResponse;
    }

    public async Task<ClaimsPrincipal> GetCurrentUserAsync()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User;
    }

    public async Task<ApiResponse<RegistrationResponse>> RegisterUserAsync(RegistrationRequestDto registrationRequest)
    {
        ApiResponse<RegistrationResponse> apiResponse = await _httpService.HttpPostAsync<RegistrationResponse>(RegistrationRequestDto.Route, registrationRequest);

        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            return apiResponse;
        }
        else
        {
            //Process validation message or exceptions
            return apiResponse;
        }
    }
    
    public async Task<ApiResponse<UpdateUserResponse>> UpdateUserAccess(Guid userId, List<UserRoleDto> selectedRoles)
    {
        EditUserRolesRequest editUserRolesRequest = new()
        {
            UserId = userId,
            //Roles = selectedRoles.Select(x => new KeyValuePair<string, IEnumerable<string>>(x.RoleName, x.Permissions.Select(p => p.PermissionName)))
            //                     .ToDictionary(x => x.Key, x => x.Value)     
        };

        ApiResponse<UpdateUserResponse> apiResponse = await _httpService.HttpPostAsync<UpdateUserResponse>("administration/usermanagement/editUserRoles",
                                                                            editUserRolesRequest);

        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            return apiResponse;
        }
        else
        {
            //Process validation messages or exceptions
            return apiResponse;
        }
    }



    public async Task<ApiResponse<UpdateUserResponse>> Save(UpdateUserRequest updateRequest)
    {
        ApiResponse<UpdateUserResponse> apiResponse = await _httpService.HttpPostAsync<UpdateUserResponse>(UpdateUserRequest.Route, updateRequest);
        if(apiResponse.StatusCode == HttpStatusCode.OK)
        {
            return apiResponse;
        }
        else
        {
            //Process validation message or exceptions
            return apiResponse;
        }
    }
}
