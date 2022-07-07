
using BlazorClient.Interfaces;

using Microsoft.AspNetCore.Components.Authorization;

using Security.Core.Models;
using Security.Core.Models.UserManagement;

namespace BlazorClient.Services;

public class UserManagementUiService : IUserManagementUiService
{
    private readonly IHttpService _httpService;

    public UserManagementUiService(IHttpService httpService)
    {
        _httpService=httpService;
    }
    
    public async Task<ApiResponse<ListUsersResponse>> ListUsers()
    {   
        return await _httpService.HttpGetAsync<ListUsersResponse>(ListUsersRequest.Route);
    }

    public async Task<ApiResponse<CreateUserResponse>> CreateAsync(CreateUserRequest createUserRequest)
    {
        return await _httpService.HttpPostAsync<CreateUserResponse>(UpdateUserRequest.Route, createUserRequest);
    }

    public async Task<ApiResponse<UpdateUserResponse>> UpdateAsnyc(UpdateUserRequest updateUserRequest)
    {
        return await _httpService.HttpPostAsync<UpdateUserResponse>(UpdateUserRequest.Route, updateUserRequest);
    }
}
