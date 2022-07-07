using System.Net;

using BlazorClient.Interfaces;
using BlazorClient.Providers;

using Microsoft.AspNetCore.Components;

using Security.Core.Models;
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.UserManagement;

namespace BlazorClient.Features.Administration.UserManagement.Components;
public partial class UserRoles
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public IRoleUiService RoleUiService { get; set; } = null!;

    [Inject]
    public IRefreshTokenUiService RefreshTokenUiService { get; set; } = null!;

    [Inject]
    public IUserManagementUiService UserManagementUiService { get; set; } = null!;

    [Inject]
    public IAuthenticationUiService AuthenticationUiService { get; set; } = null!;

    [Inject]
    protected IAppStateProvider<UserDto> StateProvider { get; set; } = null!;


    private List<RoleDto> _roleList = new();

    private UserDto User = new();

    private List<string> _messages = new();

    protected override void OnInitialized()
    {
        if (StateProvider.State != null)
        {
            User = StateProvider.State;
        }
    }

    protected override async Task OnInitializedAsync()
    {
        ApiResponse<ListRolesResponse> apiResponse = await RoleUiService.ListRoles();
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            _roleList = apiResponse?.Data?.Roles?.ToList() ?? new List<RoleDto>();
        }
    }

    protected void GrantRole(RoleDto role)
    {
        UserRoleDto userRole = new() { UserId = User.Id, RoleName = role.Name, AssignedPermissions = role.PermissionsInRole, IsDeleted = false };
        User.AssignedRoles.Add(userRole);
        StateProvider.State = User;
    }

    protected void RevokeRole(RoleDto role)
    {
        var userRoleToRemove = User.AssignedRoles.FirstOrDefault(r => r.RoleName == role.Name);

        if (userRoleToRemove != null)
        {
            userRoleToRemove.IsDeleted = true;
            StateProvider.State = User;
        }
    }

}