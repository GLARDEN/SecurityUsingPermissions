using BlazorClient.Interfaces;

using Blazored.LocalStorage;
using Security.Core.Models.Authentication;
using Security.Core.Models;

using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorClient.Handlers;

public class RefreshTokenHandler : DelegatingHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;
    private readonly IRefreshTokenUiService _refreshTokenService;
    private readonly IAuthenticationUiService _authenticationUiService;
    private readonly ILocalStorageService _localStorage;


    public RefreshTokenHandler(AuthenticationStateProvider authenticationStateProvider, NavigationManager navigationManager,
        IRefreshTokenUiService refreshTokenService, 
        IAuthenticationUiService authenticationUiService, 
        ILocalStorageService localStorage)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _navigationManager = navigationManager;
        _refreshTokenService = refreshTokenService;
        _authenticationUiService = authenticationUiService;
        _localStorage = localStorage;
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var absPath = request.RequestUri.AbsolutePath;
        if (absPath != string.Empty && !absPath.ToLower().Contains("refreshtoken") && !absPath.ToLower().Contains("login") && !absPath.ToLower().Contains("logout"))
        {
            ApiResponse<RefreshTokenResponse> response = await _refreshTokenService.TryRefreshTokenAsync();

            if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await _authenticationUiService.LogOutAsync(false);
                
                _navigationManager.NavigateTo($"login?returnUrl=" + $"{Uri.EscapeDataString(_navigationManager.Uri)}");
            }
            else
            {
                var token = await _localStorage.GetItemAsync<string>("authenticationToken");
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);
                }
            }
        }


        return await base.SendAsync(request, cancellationToken);
    }
}
