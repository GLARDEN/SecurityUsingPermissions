
using BlazorClient.Interfaces;

using Security.Core.Models;
using Security.Core.Models.Administration.RoleManagement;

using System.Net;


namespace BlazorClient.Services;

public class RoleUiService : IRoleUiService
{
    private readonly IHttpService _httpService;

    public RoleUiService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<ApiResponse<CreateRoleResponse>> CreateAsync(CreateRoleRequest createRoleRequest)
    {
        var result = await _httpService.HttpPostAsync<CreateRoleResponse>(CreateRoleRequest.Route, createRoleRequest);

        return result;
    }

    public async Task<ApiResponse<UpdateRoleResponse>> UpdateAsync(UpdateRoleRequest updateRoleRequest)
    {
        var apiResponse = await _httpService.HttpPostAsync<UpdateRoleResponse>(UpdateRoleRequest.Route, updateRoleRequest);
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

    public async Task<ApiResponse<DeleteRoleResponse>> DeleteAsync(DeleteRoleRequest deleteRoleRequest)
    {
        var apiResponse = await _httpService.HttpPostAsync<DeleteRoleResponse>(DeleteRoleRequest.Route, deleteRoleRequest);
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

    public async Task<ApiResponse<ListRolesResponse>> ListRoles()
    {
        var result = await _httpService.HttpGetAsync<ListRolesResponse>(ListRolesRequest.Route);
        return result;
    }
}


