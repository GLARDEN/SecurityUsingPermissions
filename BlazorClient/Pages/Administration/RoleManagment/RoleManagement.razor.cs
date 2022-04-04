using Microsoft.AspNetCore.Components;
using BlazorClient.Services;
using Security.Shared.Permissions.Enums;
using System.Security.Claims;
using Security.Shared.Models.Administration.RoleManagement;

namespace BlazorClient.Pages.Administration.RoleManagment;

public partial class RoleManagement:ComponentBase
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public IRoleService RoleService { get; set; }

    private IEnumerable<RoleDto> _roleList;

    public RoleManagement() { }

    protected override async Task OnInitializedAsync()
    {
        if (RoleService != null)
        {
            _roleList = await RoleService.ListRoles();
        }
    }

    public void Create()
    {
        NavigationManager.NavigateTo("CreateRole");
    }

    private void EditPermissions(RoleDto role)
    {
        NavigationManager.NavigateTo("EditPermissions");
    }

    private async Task Delete(RoleDto role)
    {

        var result = await RoleService.DeleteAsync(role);
        if (result.Success)
        {
            _roleList = await RoleService.ListRoles();
        }
    }

}