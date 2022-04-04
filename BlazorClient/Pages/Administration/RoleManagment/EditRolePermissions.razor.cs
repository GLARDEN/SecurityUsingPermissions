using BlazorClient.Services;

using Microsoft.AspNetCore.Components;

using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Helpers;

using System.Security.Claims;

namespace BlazorClient.Pages.Administration.RoleManagment;

public partial class EditRolePermissions : ComponentBase
{
    [Inject]
    public IUserService UserService { get; set; }
    [Inject]
    public IRoleService RoleService { get; set; }

    private ClaimsPrincipal _currentUser;
    protected RoleDto SelectedRole { get; set; }

    private IEnumerable<PermissionDisplay> _permissionDisplays = null!;

    protected override async Task OnInitializedAsync()
    {
        _permissionDisplays = PermissionDisplay.GetPermissionsToDisplay(typeof(Permission), true);
        _currentUser = await UserService.GetCurrentUser();

    }

    private bool IsPermissionSelected(Permission permission)
    {
        var isSeleted = false;


        return isSeleted;
    }

}
