using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using BlazorClient.Services;
using System.Net;

namespace BlazorClient.Handlers;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly string host;
    private readonly ILocalStorageService _localStorage;
    private readonly NavigationManager _navigationManager;
    private readonly ITokenService _tokenService;

    public AuthorizationMessageHandler(IWebAssemblyHostEnvironment webAssemblyHostEnvironment, ILocalStorageService localStorage, 
                                        NavigationManager navigationManager, ITokenService tokenService)
    {
        host = webAssemblyHostEnvironment.BaseAddress;
        _localStorage = localStorage;
        _navigationManager = navigationManager;
        _tokenService = tokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //If user is trying to log in or register then we don't want to check for a token
        var requestUri = request?.RequestUri?.ToString();
        var IsLoginOrRegisterRequest =  (requestUri?.IndexOf("login", StringComparison.InvariantCultureIgnoreCase) > -1 ||
                                         requestUri?.IndexOf("register", StringComparison.InvariantCultureIgnoreCase) > -1);

        var token = await _tokenService.GetTokenAsync();

        //If user is not trying to log in, validate that the token is not expired before even calling api server
        if (!IsLoginOrRegisterRequest)
        {
            //TODO: If expired, check refresh token. If refresh token expired, just redirect to login.
            var isTokenExpired = await _tokenService.IsTokenExpiredAsync(token);

            if (isTokenExpired)
            {
                //TODO: If refresh token invalid auto log user out and delete token
                _navigationManager.NavigateTo($"login?returnUrl=" + $"{Uri.EscapeDataString(_navigationManager.Uri)}");
            }
            else
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

        }
           

        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            //_navigationManager.NavigateTo("login", forceLoad: true);
            _navigationManager.NavigateTo($"login?returnUrl=" + $"{Uri.EscapeDataString(_navigationManager.Uri)}");
        }
        return response;
    }
}