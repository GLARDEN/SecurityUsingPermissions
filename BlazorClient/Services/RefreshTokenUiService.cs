using BlazorClient.Interfaces;
using Security.Core.Models.Authentication;
using Security.Core.Models;
using System.Net;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using BlazorClient.Providers;
using Security.Core.Permissions.Helpers;

namespace BlazorClient.Services;

public class RefreshTokenUiService : IRefreshTokenUiService
{
   
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IHttpService _httpService;
    private readonly ILocalStorageService _localStorageService;

    public RefreshTokenUiService(IJwtTokenService jwtTokenService, IHttpService httpService, ILocalStorageService localStorageService)
    {
        _jwtTokenService = jwtTokenService;
        _httpService = httpService;
        _localStorageService = localStorageService;
    }

    public async Task<string> GetRefreshTokenAsync()
    {
        var token = await _localStorageService.GetItemAsync<string>("refreshToken");
        return token ?? "";
    }
    public async Task RemoveRefreshTokenAsync()
    {
        await _localStorageService.RemoveItemAsync("refreshToken");
    }

    public async Task SetRefreshTokenAsync(string token)
    {
        await _localStorageService.SetItemAsync("refreshToken", token);
    }

    public async Task<ApiResponse<RevokeRefreshTokenResponse>> RevokeRefreshToken(RevokeRefreshTokenRequest revokeRefreshTokenRequest)
    {
        ApiResponse<RevokeRefreshTokenResponse> apiResponse = await _httpService.HttpPostAsync<RevokeRefreshTokenResponse>(RevokeRefreshTokenRequest.Route, revokeRefreshTokenRequest);
        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            return apiResponse;
        }
        else
        {
            //Process validation message or exceptions
            return apiResponse;
        }
    }

    public async Task<ApiResponse<RevokeRefreshTokenResponse>> RevokeRefreshTokens(RevokeRefreshTokenRequest revokeRefreshTokenRequest)
    {
        revokeRefreshTokenRequest.RevokeAll = true;
        ApiResponse<RevokeRefreshTokenResponse> apiResponse = await RevokeRefreshToken(revokeRefreshTokenRequest);
        return apiResponse;
    }

    public async Task<ApiResponse<RefreshTokenResponse>> TryRefreshTokenAsync()
    {
        ApiResponse<RefreshTokenResponse> refreshTokenApiResponse = new(HttpStatusCode.Unauthorized);

        string jwtToken = await _jwtTokenService.GetJwtTokenAsync();
               
        if(jwtToken != string.Empty)
        {            
            double minutesToExpiry = await _jwtTokenService.GetMinutesUntilTokenExpiresAsync(jwtToken);

            if (minutesToExpiry <= 2)
            {
                string refreshToken = await GetRefreshTokenAsync();

                RefreshTokenRequest refreshTokenRequest = new()
                {
                    JwtToken = jwtToken,                    
                    RefreshToken = refreshToken
                };

                refreshTokenApiResponse = await _httpService.HttpPostAsync<RefreshTokenResponse>(RefreshTokenRequest.Route, refreshTokenRequest);

                if (refreshTokenApiResponse.StatusCode == HttpStatusCode.OK)
                {
                    await _jwtTokenService.RemoveJwtTokenAsync();
                    await _jwtTokenService.SetJwtTokenAsync(refreshTokenApiResponse.Data?.JWTToken ?? "");

                    refreshTokenApiResponse.Data.IsAuthenticationSuccessful = true;
                    return refreshTokenApiResponse;
                }
                else
                {
                    // await RemoveRefreshTokenAsync();
                    //Process validation messages or exceptions             
                }
            }
            else
            {
                refreshTokenApiResponse.StatusCode = HttpStatusCode.OK;
            }
        }
     
        return refreshTokenApiResponse;
    }
}
