using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using BlazorClient.Services;
using System.Security.Principal;
using BlazorClient.Interfaces;
using System.Net.Http;

namespace BlazorClient.Providers;

public class AuthStateProvider : AuthenticationStateProvider
{    

    private readonly IJwtTokenService _jwtTokenService;    
      private readonly AuthenticationState _anonymousAuthState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));


    public AuthStateProvider( IJwtTokenService jwtTokenService) 
    {
        _jwtTokenService = jwtTokenService;        
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity = new ClaimsIdentity();

        string jwtToken = await _jwtTokenService.GetJwtTokenAsync();

        if (String.IsNullOrEmpty(jwtToken) || await _jwtTokenService.IsJwtTokenExpiredAsync(jwtToken))
        {
            return _anonymousAuthState;
        }

        try
        {
            IEnumerable<Claim> claims = _jwtTokenService.GetClaimsFromJwtToken(jwtToken);
            identity = new ClaimsIdentity(claims, "jwtAuthType");
        }
        catch
        {
            return _anonymousAuthState;
        }


        ClaimsPrincipal user = new ClaimsPrincipal(identity);
        AuthenticationState authenticationState = new AuthenticationState(user);

        return authenticationState;
    }
    public async Task NotifyUserAuthenticationAsync()
    {
       NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
     
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymousAuthState);
        NotifyAuthenticationStateChanged(authState);
        
    }
}
