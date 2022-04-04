using Microsoft.AspNetCore.Components;
using BlazorClient.Services;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions;
using Security.Shared.Models.UserManagement;

namespace BlazorClient.Pages.Administration.UserManagement;


public partial class Users 
{
    private string PageTitle { get; set; }

    [Inject]
    private IUserService UserService { get; set; }

    private List<UserSummaryDto> _userList;
    
    public Users()
    {
        PageTitle = "Manage Users";
    }

    protected override async Task OnInitializedAsync()
    {
      _userList = await UserService.ListUsers();
    }
}
