using BlazorClient.Providers;
using Security.Shared.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Security.Shared.Models.Authentication;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlazorClient.Services;

public class UserService : IUserService
{
    private readonly IHttpService _httpService;
    private readonly JsonSerializerOptions _options;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ITokenService _tokenService;

    public UserService(IHttpService httpService, AuthenticationStateProvider authStateProvider, ITokenService tokenService)
    {
        _httpService=httpService;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };        
        _authStateProvider = authStateProvider;
        _tokenService = tokenService;
    }

    public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest changePasswordRequest)
    {
        ChangePasswordResponse changePasswordResponse = await _httpService.HttpPostAsync<ChangePasswordResponse>("authentication/changepassword",changePasswordRequest);

        return changePasswordResponse;
    }


    public async Task<bool> IsUserAuthenticated()
    {   
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
      
        var authResult = await _httpService.HttpPostAsync<LoginResponseDto>("authentication/login", loginRequestDto);
        
        if (!authResult.IsAuthenticationSuccessful)
        {
            return authResult;
        }

        await _tokenService.SetTokenAsync(authResult.Token);
        await _authStateProvider.GetAuthenticationStateAsync();

        authResult.IsAuthenticationSuccessful = true;
        return authResult;
    }
    public async Task Logout()
    {
        await _tokenService.RemoveTokenAsync();
        _httpService.LogOut();
        _authStateProvider.GetAuthenticationStateAsync();        
    }

    public async Task<RegistrationResponseDto> RegisterUserAsync(RegistrationRequestDto registrationRequest)
    {
  
        RegistrationResponseDto registrationResult = await _httpService.HttpPostAsync<RegistrationResponseDto>("authentication/register", registrationRequest);
  
        return registrationResult;
    }

    public async Task<ClaimsPrincipal>  GetCurrentUser()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User;
    }
}
