using BlazorClient.Providers;
using BlazorClient.Services;

using Microsoft.AspNetCore.Components;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Permissions.Enums;
using Security.Core.Permissions.Helpers;

using System.Security.Claims;

namespace BlazorClient.Pages.Administration.RoleManagement;

public partial class EditRolePermissions : ComponentBase
{
    [Inject]
    protected IAppStateProvider<RoleDto> StateProvider { get; set; } = null!;
    [Inject]
    public IRoleService RoleService { get; set; }

    private ClaimsPrincipal _currentUser;
    protected RoleDto SelectedRole { get; set; }

    private IEnumerable<PermissionDisplay> _permissionDisplays = null!;

    protected override void OnInitialized()
    {
        SelectedRole =StateProvider.State;

        StateProvider.State = null;
    }

    protected override async Task OnInitializedAsync()
    {
        _permissionDisplays = PermissionDisplay.GetPermissionsToDisplay(typeof(Permission), true);
    }
    private void ChangePropertyValue()
    {
        SelectedRole = StateProvider.State;
    }

    private bool IsPermissionSelected(Permission permission)
    {
        var isSeleted = false;


        return isSeleted;
    }

}
