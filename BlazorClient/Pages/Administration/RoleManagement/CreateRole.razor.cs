using BlazorClient.Services;

using Microsoft.AspNetCore.Components;

using Security.Shared.Models.Administration.Role;

namespace BlazorClient.Pages.Administration.RoleManagement;

public partial class CreateRole
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public IRoleService RoleService { get; set; }

    private CreateRoleRequest _newRole = new();


    public CreateRole() { }

    protected override async Task OnInitializedAsync()
    {

    }
    private async Task SaveRole()
    {
        //var result = await RoleService.CreateAsync(_newRole);
        //if (result.Success)
        //{
        //  NavigationManager.NavigateTo("rolemanagement");
        //}
    }

}
