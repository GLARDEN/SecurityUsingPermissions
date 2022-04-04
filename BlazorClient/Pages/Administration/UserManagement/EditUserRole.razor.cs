using BlazorClient.Services;
using Microsoft.AspNetCore.Components;

using Security.Shared.Models;
using Security.Shared.Models.UserManagement;
using Security.Shared.Permissions.Extensions;

namespace BlazorClient.Pages.Administration.UserManagement;
public partial class EditUserRole
{
    [Inject]
    public IPermissionDisplayService PermissionDisplayService { get; set; } = null!;

    [Inject]
    public IUserService UserService { get; set; } = null!;

    [Parameter]
    public EditUserRequest SelectedUser { get; set; }    

    [Parameter]
    public EventCallback<EditUserRequest> SelectedUserChanged { get; set; } 

    [Parameter]
    public bool IsVisible { get; set; }

    [Parameter]
    public EventCallback<bool> IsVisibleChanged { get; set; }

    private List<RoleDisplayDto> _rolesDisplay;

    protected override async Task OnInitializedAsync()
    {
        _rolesDisplay = PermissionDisplayService.GetRolesForDisplay();

        //SelectedUser.RoleNames.ForEach(ur =>
        //{
            //var permissionNames = ur.Permissions.ConvertPackedPermissionToNames();

            //_rolesDisplay.ForEach(role => {
                
            //    role.Permissions.ForEach(p => {
            //        p.IsSelected = permissionNames.Contains(p.PermissionName);
            //    });
            //    role.IsSelected = role.Permissions.Any(p => p.IsSelected);
            //});
        //});
        
    }

    protected void CheckboxClicked(string roleName, PermissionInfoDto selectedPermission, Object checkedValue)
    {  
       
        selectedPermission.IsSelected = (bool)checkedValue;
        
        bool roleSelected = _rolesDisplay.Any(r => r.RoleName == roleName &&  r.Permissions.Any(p => p.IsSelected));        
        _rolesDisplay.FirstOrDefault(r => r.RoleName == roleName).IsSelected = roleSelected;

    }

    protected async Task SaveAsync()
    {
        var selectedRoles = _rolesDisplay.Where(r => r.Permissions.Any(p => p.IsSelected)).ToList();

        var result = await UserService.UpdateUserAccess(SelectedUser.Id, selectedRoles);
        await SelectedUserChanged.InvokeAsync(SelectedUser);
        IsVisible = !IsVisible;
        await IsVisibleChanged.InvokeAsync(IsVisible);
    }
}

