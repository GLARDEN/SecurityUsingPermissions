using Microsoft.AspNetCore.Components;
using BlazorClient.Services;

using Security.Shared.Models.Administration.RoleManagement;
using System;
using BlazorClient.Providers;
using Security.Shared.Models.Administration.Role;

namespace BlazorClient.Pages.Administration.RoleManagement;

public partial class Roles
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject] 
    protected IAppStateProvider<RoleDto> StateProvider { get; set; } = null!;

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

    private void CreateNewRole()
    {
       StateProvider.State =null;
       NavigationManager.NavigateTo("/RoleManagement/CreateOrEditRole");
    }
    private void EditRole(RoleDto roleDto)
    {
        StateProvider.State = roleDto;
        NavigationManager.NavigateTo("RoleManagement/CreateOrEditRole");
    }

    private async Task Delete(RoleDto role)
    {
        DeleteRoleRequest deleteRoleRequest = new()
        {
            RoleId = role.Id
        };


        await RoleService.DeleteAsync(deleteRoleRequest);
       
        _roleList.Remove(role);
       
    }

    public void Dispose()
    {
        StateProvider.OnStateChange -= StateHasChanged;
    }
}