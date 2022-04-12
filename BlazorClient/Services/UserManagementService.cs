using System.Text.Json;

using Microsoft.AspNetCore.Components.Authorization;

using Security.Core.Models.UserManagement;

namespace BlazorClient.Services;

public class UserManagementService : IUserManagementService
{
    private readonly IHttpService _httpService;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ITokenService _tokenService;

    public UserManagementService(IHttpService httpService, AuthenticationStateProvider authStateProvider, ITokenService tokenService)
    {
        _httpService=httpService;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };        
        _authStateProvider = authStateProvider;
        _tokenService = tokenService;
    }

    
    public async Task<List<UserSummaryDto>> ListUsers()
    {   
        return (await _httpService.HttpGetAsync<ListUsersResponse>(ListUsersRequest.Route)).RegisteredUsers;
    }

    public async Task<UserDto> UpdateUserAccess(UpdateUserRequest updateUserRequest)
    {
        return (await _httpService.HttpPostAsync<UpdateUserResponse>(UpdateUserRequest.Route, updateUserRequest)).User;
    }
}
