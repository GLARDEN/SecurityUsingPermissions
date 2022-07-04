using Microsoft.AspNetCore.Components;
using Security.Core.Models.UserManagement;
using Security.Core.Models;
using System.Net;
using BlazorClient.Interfaces;
using BlazorClient.Providers;
using Security.Core.Models.Administration.RoleManagement;
using BlazorClient.Services;

namespace BlazorClient.Pages.Administration.UserManagement;


public partial class Users
{
    private string PageTitle { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IAuthenticationUiService UserService { get; set; }

    [Inject]
    private IUserManagementUiService UserManagementUiService { get; set; }

    [Inject]
    protected IAppStateProvider<UserDto> StateProvider { get; set; } = null!;

    private List<UserDto>? _userList;

    private UserDto _user = new();
    
    private void Edit(UserDto user)
    {
        StateProvider.State = user;
        NavigationManager.NavigateTo("UserManagement/CreateOrEditUser");
    }

    private async Task Delete(UserDto user)
    {
        //DeleteRoleRequest deleteRoleRequest = new()
        //{
        //    RoleId = role.Id
        //};


       // await RoleUiService.DeleteAsync(deleteRoleRequest);

       // _roleList.Remove(role);

    }

    public Users()
    {
        PageTitle = "Manage Users";
       
    }
    protected override void OnInitialized()
    {
   
        if (StateProvider.State != null)
        {
            _user = StateProvider.State;
        }

        StateProvider.OnStateChange += StateHasChanged;
    }
    protected override async Task OnInitializedAsync()
    {
        ApiResponse<ListUsersResponse> apiResponse = await UserManagementUiService.ListUsers();
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            _userList = apiResponse.Data?.RegisteredUsers.ToList();
        }
        else
        {
            //manage error or validation messages from api response
        }
    }

    public void Dispose()
    {
        StateProvider.OnStateChange -= StateHasChanged;
    }
}
