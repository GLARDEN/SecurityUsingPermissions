using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Security.Core.Models;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;

namespace BlazorClient.Services;

public class UserService : IUserService
{
    private readonly IHttpService _httpService;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ITokenService _tokenService;

    public UserService(IHttpService httpService, AuthenticationStateProvider authStateProvider, ITokenService tokenService)
    {
        _httpService=httpService;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };        
        _authStateProvider = authStateProvider;
        _tokenService = tokenService;
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        ChangePasswordResponse changePasswordResponse = await _httpService.HttpPostAsync<ChangePasswordResponse>("authentication/changepassword",changePasswordRequest);

        return changePasswordResponse;
    }

    public async Task<bool> IsUserAuthenticated()
    {   
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
      
        var authResult = await _httpService.HttpPostAsync<LoginResponseDto>(LoginRequestDto.Route, loginRequestDto);
        
        if (!authResult.IsAuthenticationSuccessful)
        {
            return authResult;
        }

        await _tokenService.SetTokenAsync(authResult.Token);
        await _authStateProvider.GetAuthenticationStateAsync();

        authResult.IsAuthenticationSuccessful = true;
        return authResult;
    }

    public async Task Logout()
    {
        await _tokenService.RemoveTokenAsync();
        _httpService.LogOut();
        _authStateProvider.GetAuthenticationStateAsync();        
    }

    public async Task<RegistrationResponseDto> RegisterUserAsync(RegistrationRequestDto registrationRequest)
    {
        return (await _httpService.HttpPostAsync<RegistrationResponseDto>(RegistrationRequestDto.Route, registrationRequest));
    }

    public async Task<ClaimsPrincipal>  GetCurrentUser()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User;
    }

    public async Task<Guid> GetUserIdAsync()
    {        
        var currentUser = await GetCurrentUser();

        var userId = currentUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        return Guid.Parse(userId?.Value ?? "");
    }

    public async Task<List<UserSummaryDto>> ListUsers()
    {   
        return (await _httpService.HttpGetAsync<ListUsersResponse>(ListUsersRequest.Route)).RegisteredUsers;
    }

    public async Task<UpdateUserResponse> UpdateUserAccess(Guid userId, List<UserRoleDto> selectedRoles)
    {
        EditUserRolesRequest editUserRolesRequest = new()
        {
            UserId = userId,
            //Roles = selectedRoles.Select(x => new KeyValuePair<string, IEnumerable<string>>(x.RoleName, x.Permissions.Select(p => p.PermissionName)))
            //                     .ToDictionary(x => x.Key, x => x.Value)     
        };

        var result = await _httpService.HttpPostAsync<UpdateUserResponse>("administration/usermanagement/editUserRoles", editUserRolesRequest);

        return result;
    }

    public async Task<UpdateUserResponse> Save(UpdateUserRequest updateRequest)
    {

        var updateResult = await _httpService.HttpPostAsync<UpdateUserResponse>("administration/usermanagement/updateUser", updateRequest);
               
        return updateResult;
    }
}
