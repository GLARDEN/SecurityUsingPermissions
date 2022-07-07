using Ardalis.Result;

using BlazorClient.Interfaces;
using BlazorClient.Providers;

using Microsoft.AspNetCore.Components;

using Security.Core.Models;
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Permissions.Services;

using System.Data;
using System.Net;

namespace BlazorClient.Features.Administration.RoleManagement;

public partial class CreateOrEditRole : IDisposable
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IAppStateProvider<RoleDto> StateProvider { get; set; } = null!;

    [Inject]
    public IPermissionService PermissionDisplayService { get; set; } = null!;

    [Inject]
    public IRoleUiService RoleUiService { get; set; }

    private string PageTitle = "";

    private List<PermissionGroupDto> _groupedPermissions;

    private RoleDto _role = new();

    private IEnumerable<string> _permissionGroupNames = new List<string>();

    private string currentGroupName = string.Empty;

    private List<string>? _messages = new();

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
        try
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

                var apiResponse = await RoleUiService.CreateAsync(createRoleRequest);

                if (apiResponse.StatusCode != HttpStatusCode.BadRequest)
                {
                    NavigationManager.NavigateTo("/RoleManagement/Roles", false);
                }
                else
                {
                    _messages = apiResponse.ResponseMessages;
                }
            }
            else
            {
                _role.PermissionsInRole = _selectedPermissions;
                UpdateRoleRequest request = UpdateRoleRequest.FromDto(_role);
                ApiResponse<UpdateRoleResponse> apiResponse = await RoleUiService.UpdateAsync(request);

                if (apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    _messages = apiResponse.ResponseMessages;
                }

                NavigationManager.NavigateTo("/RoleManagement/Roles");
            }
        }
        catch (Exception ex)
        {
            var result = ex.InnerException.Message;
        }

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


    public void Dispose()
    {
        StateProvider.State = null;      
    }
}
