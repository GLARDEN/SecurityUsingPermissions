using BlazorClient.Providers;
using BlazorClient.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Extensions;

using System.Data;

namespace BlazorClient.Pages.Administration.RoleManagement;

public partial class CreateOrEditRole
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IAppStateProvider<RoleDto> StateProvider { get; set; } = null!;

    [Inject]
    public IPermissionDisplayService PermissionDisplayService { get; set; } = null!;

    [Inject]
    public IRoleService RoleService { get; set; }

    private string PageTitle = "";

    private List<PermissionGroupDto> _groupedPermissions;

    private RoleDto _role = new();

    private IEnumerable<string> _permissionGroupNames = new List<string>();

    private string currentGroupName = string.Empty;

    
    public CreateOrEditRole() { }

    protected override void OnInitialized()
    {
        _groupedPermissions = PermissionDisplayService.GroupPermissionsForDisplay();

        if (StateProvider.State != null)
        {
            _role = StateProvider.State;
        }

        if (_role.Id.Equals(Guid.Empty))
        {
            PageTitle = "Create New Role";
        }
        else
        {
            _groupedPermissions.ForEach(gp =>
            {
                gp.Permissions.ForEach(p => 
                {
                    p.IsSelected = _role.PermissionsInRole.Contains(p.PermissionName);
                });
            });

        PageTitle = $"Edit {_role.Name} Role";
        }

    }

    private async Task SaveRole()
    {
        var _selectedPermissions = GetSelectedPermissions();

        if (_role.Id.Equals(Guid.Empty))
        {
            CreateRoleRequest createRoleRequest = new()
            {
                Name = _role.Name,
                Description = _role.Description,
                Permissions = _selectedPermissions
            };

            await RoleService.CreateAsync(createRoleRequest);
        }
        else
        {
            _role.PermissionsInRole = _selectedPermissions;

            UpdateRoleRequest request = UpdateRoleRequest.FromDto(_role);
            await RoleService.UpdateAsync(request);
        }

        NavigationManager.NavigateTo("/RoleManagement/Roles");
    }

    private IEnumerable<string> GetSelectedPermissions()
    {
        List<string> _selectedPermissions = new();

        _groupedPermissions.ForEach(gp =>
        {
            var permissions = gp.Permissions.Where(p => p.IsSelected).Select(p => p.PermissionName).ToList();
            _selectedPermissions.AddRange(permissions);
        });

        return _selectedPermissions;
    }
}
