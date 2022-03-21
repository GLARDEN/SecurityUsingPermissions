using BlazorClient.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.Pages;

public partial class Logout
{
    [Inject]
    public IUserService UserService { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        await UserService.Logout();
        
        NavigationManager.NavigateTo("/");
    }
}