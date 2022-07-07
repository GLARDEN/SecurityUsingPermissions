using BlazorClient.Interfaces;

using Microsoft.AspNetCore.Components;

namespace BlazorClient.Features;

public partial class Logout : ComponentBase
{
    [Inject]
    public IAuthenticationUiService AuthenticationUiService { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {

        await AuthenticationUiService.LogOutAsync(false);
        
        NavigationManager.NavigateTo("/");
    }
}