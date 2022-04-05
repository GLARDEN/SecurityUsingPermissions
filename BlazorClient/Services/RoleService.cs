using BlazorClient.Providers;

using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.Authentication;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Helpers;

namespace BlazorClient.Services;

public class RoleService : IRoleService
{
    private readonly IHttpService _httpService;

    public RoleService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<CreateRoleResponse> CreateAsync(CreateRoleRequest createRoleRequest)
    {
        var createRoleResponse = await _httpService.HttpPostAsync<CreateRoleResponse>(CreateRoleRequest.Route, createRoleRequest);

        if (!createRoleResponse.Success)
        {
            return null;
        }

        return createRoleResponse;
    }

    public async Task<DeleteRoleResponse> DeleteAsync(RoleDto role)
    {
        DeleteRoleRequest deleteRoleRequest = new()
        {
            Role = role
        };

        var deleteRoleResponse = await _httpService.HttpPostAsync<DeleteRoleResponse>(DeleteRoleRequest.Route, deleteRoleRequest);

        if (!deleteRoleResponse.Success)
        {
            return null;
        }

        return deleteRoleResponse;
    }

    public async Task<List<RoleDto>> ListRoles()
    {
       return (await _httpService.HttpGetAsync<ListRolesResponse>(ListRolesRequest.Route)).Roles.ToList();

    }
}
