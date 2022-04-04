using BlazorClient.Services;

using Microsoft.AspNetCore.Components;

using Security.Shared.Models.Administration.Role;
using Security.Shared.Permissions.Enums;

namespace BlazorClient.Pages.Administration.RoleManagment
{
    public partial class CreateRole : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        public IRoleService RoleService { get; set; }

        private CreateRoleRequest _newRole = new();


        public CreateRole() { }

        private async Task SaveRole()
        {
            //var result = await RoleService.CreateAsync(_newRole);
            //if (result.Success)
            //{
              //  NavigationManager.NavigateTo("rolemanagement");
            //}
        }

    }
}