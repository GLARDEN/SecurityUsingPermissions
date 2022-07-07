using BlazorClient.Interfaces;
using BlazorClient.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

using Security.Core.Models;
using Security.Core.Models.Authentication;

using System.Linq;
using System.Net;

namespace BlazorClient.Features;

public partial class Login : IDisposable
{        
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    public IAuthenticationUiService AuthenticationUiService { get; set; } = null!;


    public bool ShowAuthenticationError { get; set; }
    public List<string> Messages { get; set; } = new();

    private LoginRequest _loginRequest = new LoginRequest();
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
        ShowAuthenticationError = false;
        Messages.Clear();
        ApiResponse<LoginResponse> apiResponse = await AuthenticationUiService.Login(_loginRequest);
        if (apiResponse.StatusCode == HttpStatusCode.Unauthorized)
        {
            ShowAuthenticationError = true;
            Messages.Add("Login attempt failed.");


        }
        else
        {
            NavigationManager.NavigateTo(returnURL);
        }
    }
    public void Dispose()
    {  
     
    }
}