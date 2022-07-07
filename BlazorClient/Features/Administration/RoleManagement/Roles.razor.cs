using Ardalis.Result;

using BlazorClient.Interfaces;
using BlazorClient.Providers;
using BlazorClient.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

using Security.Core.Models;
using Security.Core.Models.Administration.RoleManagement;

using System.Net;

namespace BlazorClient.Features.Administration.RoleManagement;

public partial class Roles : IDisposable
{
    [CascadingParameter]
    public Task<AuthenticationState> AuthenticationState { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    protected IAppStateProvider<RoleDto> StateProvider { get; set; } = null!;

    [Inject]
    public IRoleUiService RoleUiService { get; set; }

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
        ApiResponse<ListRolesResponse> apiResponse = await RoleUiService.ListRoles();
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            _roleList = apiResponse?.Data?.Roles?.ToList() ?? new List<RoleDto>();
        }
    }

    private void CreateNewRole()
    {
        StateProvider.State = null;
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


        await RoleUiService.DeleteAsync(deleteRoleRequest);

        _roleList.Remove(role);

    }

    public void Dispose() => StateProvider.OnStateChange -= StateHasChanged;
}