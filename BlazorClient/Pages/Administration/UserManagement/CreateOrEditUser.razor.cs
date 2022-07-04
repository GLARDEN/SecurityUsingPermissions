using BlazorClient.Interfaces;
using BlazorClient.Providers;
using BlazorClient.Services;

using Microsoft.AspNetCore.Components;

using Security.Core.Models;
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;

using System.Net;

namespace BlazorClient.Pages.Administration.UserManagement;
public partial class CreateOrEditUser : IDisposable
{
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

   // [Inject]
   // public IHttpInterceptorService Interceptor { get; set; }

    [Inject]
    public IRoleUiService RoleUiService { get; set; } = null!;

    [Inject]
    public IRefreshTokenUiService RefreshTokenUiService { get; set; } = null!;

    [Inject]
    public IUserManagementUiService UserManagementUiService { get; set; } = null!;

    [Inject]
    public IAuthenticationUiService AuthenticationUiService { get; set; } = null!;

    [Inject]
    protected IAppStateProvider<UserDto> StateProvider { get; set; } = null!;


    private List<RoleDto> _roleList = new();

    private UserDto _selectedUser = new();

    private List<string> _messages = new();


    protected override void OnInitialized()
    {
        //Interceptor.RegisterEvent();
        if (StateProvider.State != null)
        {
            _selectedUser = StateProvider.State;
        }
    }
    protected override async Task OnInitializedAsync()
    {
        ApiResponse<ListRolesResponse> apiResponse = await RoleUiService.ListRoles();
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            _roleList = apiResponse?.Data?.Roles?.ToList() ?? new List<RoleDto>();
        }
    }

    protected string GetValidStatusIcon(RefreshTokenDto refreshToken)
    {
        if (refreshToken.IsInvalid)
        {
            return "fa-solid fa-x fa-2xl center text-danger";
        }
        else
        {
            return "fa-solid fa-check fa-2xl center text-success";
        }
    }

    protected void CheckboxClicked(string roleName, PermissionInfoDto selectedPermission, Object checkedValue)
    {
        //bool roleSelected = _rolesDisplay.Any(r => r.RoleName == roleName &&  r.Permissions.Any(p => p.IsSelected));        
        //_rolesDisplay.FirstOrDefault(r => r.RoleName == roleName).IsSelected = roleSelected;

    }

    protected void GrantRole(RoleDto role)
    {
        UserRoleDto userRole = new() { UserId = _selectedUser.Id, RoleName = role.Name, AssignedPermissions = role.PermissionsInRole, IsDeleted = false };
        _selectedUser.AssignedRoles.Add(userRole);
    }

    protected void RevokeRole(RoleDto role)
    {
        var userRoleToRemove = _selectedUser.AssignedRoles.FirstOrDefault(r => r.RoleName == role.Name);

        if (userRoleToRemove != null)
        {
            userRoleToRemove.IsDeleted = true;
        }

    }

    protected async Task RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {   
        RevokeRefreshTokenRequest revokeRefreshTokenRequest = new()
        {
            UserId = _selectedUser.Id,
            DeviceId = (await AuthenticationUiService.GetUserDeviceIdAsync()).DeviceId
        };

        ApiResponse<RevokeRefreshTokenResponse> apiResponse = await RefreshTokenUiService.RevokeRefreshToken(revokeRefreshTokenRequest);
        
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            refreshTokenDto.IsInvalid = true;
            StateHasChanged();
        }
        else
        {
            _messages = apiResponse.ResponseMessages ?? new List<string>();
        }
    }

    protected async Task RevokeAllTokens()
    {
        RevokeRefreshTokenRequest revokeRefreshTokenRequest = new()
        {
            UserId = _selectedUser.Id,
            RevokeAll = true
        };

        ApiResponse<RevokeRefreshTokenResponse> apiResponse = await RefreshTokenUiService.RevokeRefreshTokens(revokeRefreshTokenRequest);

        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            _selectedUser.RefreshTokens.ForEach(t => t.IsInvalid = true);
            StateHasChanged();
        }
        else
        {
            _messages = apiResponse.ResponseMessages ?? new List<string>();
        }
    }

    
    protected async Task SaveAsync()
    {
        try
        {
            if (_selectedUser.Id.Equals(Guid.Empty))
            {
                CreateUserRequest createRoleRequest = new()
                {
                    Email = _selectedUser.Email,
                    AssignedRoles = _selectedUser.AssignedRoles
                };

                var apiResponse = await UserManagementUiService.CreateAsync(createRoleRequest);

                if (apiResponse.StatusCode != HttpStatusCode.BadRequest)
                {
                    NavigationManager.NavigateTo("/UserManagement/Users", false);
                }
                else
                {
                    _messages = apiResponse.ResponseMessages ?? new List<string>();
                }
            }
            else
            {
                UpdateUserRequest updateUserRequest = new();
                updateUserRequest.Id = _selectedUser.Id;
                updateUserRequest.Email = _selectedUser.Email;
                updateUserRequest.Roles = _selectedUser.AssignedRoles;

                ApiResponse<UpdateUserResponse> apiResponse = await UserManagementUiService.UpdateAsnyc(updateUserRequest);

                if (apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    _messages = apiResponse.ResponseMessages ?? new List<string>();
                }

                NavigationManager.NavigateTo("/UserManagement/Users");
            }
        }
        catch (Exception ex)
        {
            //Add Logging
            _messages.Add(ex?.InnerException?.Message ?? "");
        }
    }

    public void Dispose()
    {   
      //  Interceptor.DisposeEvent();
    }
}

