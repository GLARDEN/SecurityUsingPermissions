using System.Net;

using BlazorClient.Interfaces;
using Microsoft.AspNetCore.Components;
using Security.Core.Models.Authentication;

using Security.Core.Models;
using BlazorClient.Providers;
using Security.Core.Models.UserManagement;
using Autofac.Core;

namespace BlazorClient.Features.Administration.UserManagement.Components;

public partial class TrustedDevices
{

    [Inject]
    public IRefreshTokenUiService RefreshTokenUiService { get; set; } = null!;

    [Inject]
    protected IAppStateProvider<UserDto> StateProvider { get; set; } = null!;

    private UserDto User = new();

    public TrustedDevices()
    {

    }

    protected override void OnInitialized()
    {
        if (StateProvider.State != null)
        {
            User = StateProvider.State;
        }
    }

    private List<string> _messages = new();
    protected async Task RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
    {
        RevokeRefreshTokenRequest revokeRefreshTokenRequest = new()
        {
            UserId = User.Id,
            DeviceId = refreshTokenDto.DeviceId
        };

        ApiResponse<RevokeRefreshTokenResponse> apiResponse = await RefreshTokenUiService.RevokeRefreshToken(revokeRefreshTokenRequest);

        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            refreshTokenDto.IsValid = false;
            User.RefreshTokens.FirstOrDefault(t => t.Token.Equals(refreshTokenDto.Token)).IsValid = false;
            StateProvider.State = User;
            StateHasChanged();
        }
        else
        {
            _messages = apiResponse.ResponseMessages ?? new List<string>();
        }
    }

    protected async Task RevokeAllTokens()
    {
        RevokeRefreshTokenRequest revokeRefreshTokenRequest = new()
        {
            UserId = User.Id,
            RevokeAll = true
        };

        ApiResponse<RevokeRefreshTokenResponse> apiResponse = await RefreshTokenUiService.RevokeRefreshTokens(revokeRefreshTokenRequest);

        if (apiResponse.StatusCode == HttpStatusCode.OK)
        {
            User.RefreshTokens.ForEach(t => t.IsValid = false);
            StateProvider.State = User;
        }
        else
        {
            _messages = apiResponse.ResponseMessages ?? new List<string>();
        }
    }

    public void Dispose() => StateProvider.State = null;
}
