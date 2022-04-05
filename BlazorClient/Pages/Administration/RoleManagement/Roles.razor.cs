using Microsoft.AspNetCore.Components;
using BlazorClient.Services;

using Security.Shared.Models.Administration.RoleManagement;
using System;
using BlazorClient.Providers;

namespace BlazorClient.Pages.Administration.RoleManagement;

public partial class Roles
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] 
    protected StateProvider StateProvider { get; set; } = null!;

    [Inject]
    public IRoleService RoleService { get; set; }

    private string PageTitle { get; set; }

    protected List<RoleDto> _roleList;

    public Roles()
    {
        PageTitle = "Role Management";
    }

    protected override void OnInitialized()
    {
        StateProvider.OnStateChange += StateHasChanged;
    }


    protected override async Task OnInitializedAsync()
    {
        _roleList = await RoleService.ListRoles();
    }

    public void Create()
    {

    }

    private void EditPermissions(RoleDto role)
    {
        StateProvider.State = role;
        NavigationManager.NavigateTo("RoleManagement/EditRolePermissions");
    }

    private async Task Delete(RoleDto role)
    {

        var result = await RoleService.DeleteAsync(role);
        if (result.Success)
        {
            _roleList = await RoleService.ListRoles();
        }
    }

    public void Dispose()
    {
        StateProvider.OnStateChange -= StateHasChanged;
    }
}