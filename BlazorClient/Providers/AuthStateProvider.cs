using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using BlazorClient.Services;
using System.Security.Principal;
using Security.Shared.Permissions.Constants;
using Security.Shared.Permissions.Helpers;

namespace BlazorClient.Providers;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorageService;
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;
    private readonly AuthenticationState _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public AuthStateProvider(IHttpClientFactory httpClientFactory, ILocalStorageService localStorageService, ITokenService tokenService)
    {
        _httpClient = httpClientFactory.CreateClient("WebAPI");
        _localStorageService = localStorageService;
        _tokenService = tokenService;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity = new ClaimsIdentity();
    
        string token = await _tokenService.GetTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (!String.IsNullOrEmpty(token))
        {
            try
            {
                bool isTokenExpired = await _tokenService.IsTokenExpiredAsync(token);
                if (isTokenExpired)
                {
                    await _tokenService.RemoveTokenAsync();
                    identity = new ClaimsIdentity();
                }
                else
                {
                    IEnumerable<Claim> claims = await _tokenService.GetClaimsFromTokenAsync(token);
                  
                    identity = new ClaimsIdentity(claims, "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
                }
            }
            catch
            {
                await _tokenService.RemoveTokenAsync();
                identity = new ClaimsIdentity();                
            }
        }

        ClaimsPrincipal user = new ClaimsPrincipal(identity);
        AuthenticationState authenticationState = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));

        return authenticationState;
    }

   
}
