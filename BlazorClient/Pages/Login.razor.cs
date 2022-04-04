using BlazorClient.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

using Security.Shared.Models.Authentication;

namespace BlazorClient.Pages;

public partial class Login : ComponentBase
{
    private LoginRequestDto _loginRequest = new LoginRequestDto();
    [Inject]
    public IUserService UserService { get; set; } = null!;
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    public bool ShowAuthError { get; set; }
    public string Error { get; set; }
    private string returnURL = string.Empty;

    protected override void OnInitialized()
    {   
        var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl", out var url))
        {
            returnURL = url;
        }
    }
    public async Task ExecuteLogin()
    {
        ShowAuthError = false;
        var result = await UserService.Login(_loginRequest);
        if (!result.IsAuthenticationSuccessful)
        {
            Error = result.ErrorMessage;
            ShowAuthError = true;
        }
        else
        {
            NavigationManager.NavigateTo(returnURL);
        }
    }
}
