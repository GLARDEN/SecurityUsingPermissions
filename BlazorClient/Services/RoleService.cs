using BlazorClient.Providers;

using Security.Core.Models.Administration.RoleManagement;


namespace BlazorClient.Services;

public class RoleService : IRoleService
{
    private readonly IHttpService _httpService;

    public RoleService(IHttpService httpService)
    {
        _httpService = httpService;
    }

    public async Task<RoleDto> CreateAsync(CreateRoleRequest createRoleRequest)
    {
        return (await _httpService.HttpPostAsync<CreateRoleResponse>(CreateRoleRequest.Route, createRoleRequest)).Role;
    }

    public async Task<RoleDto> UpdateAsync(UpdateRoleRequest updateRoleRequest)
    {
        return (await _httpService.HttpPostAsync<UpdateRoleResponse>(UpdateRoleRequest.Route, updateRoleRequest)).Role;
    }

    public async Task DeleteAsync(DeleteRoleRequest deleteRoleRequest)
    {        
       await _httpService.HttpPostAsync<DeleteRoleResponse>(DeleteRoleRequest.Route, deleteRoleRequest);
    }

    public async Task<List<RoleDto>> ListRoles()
    {
        return (await _httpService.HttpGetAsync<ListRolesResponse>(ListRolesRequest.Route)).Roles.ToList();

    }
}


