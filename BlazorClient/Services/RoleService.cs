using BlazorClient.Providers;

using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.Authentication;
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
        var createRoleResponse = await _httpService.HttpPostAsync<CreateRoleResponse>("administration/role/create", createRoleRequest);

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

        var deleteRoleResponse = await _httpService.HttpPostAsync<DeleteRoleResponse>("administration/role/delete", deleteRoleRequest);

        if (!deleteRoleResponse.Success)
        {
            return null;
        }

        return deleteRoleResponse;
    }


    public async Task<IEnumerable<RoleDto>> ListRoles()
    {
        var listRoleResponse = await _httpService.HttpGetAsync<ListRolesResponse>("administration/role/list");

        if (!listRoleResponse.Success)
        {
            return null;
        }
        
        return listRoleResponse.Roles;
    }

  
}
