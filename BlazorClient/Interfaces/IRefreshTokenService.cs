using Security.Core.Models.Authentication;
using Security.Core.Models;

namespace BlazorClient.Interfaces;

public interface IRefreshTokenUiService
{
    Task<string> GetRefreshTokenAsync();
    Task RemoveRefreshTokenAsync();
    Task SetRefreshTokenAsync(string token);
    Task<ApiResponse<RevokeRefreshTokenResponse>> RevokeRefreshToken(RevokeRefreshTokenRequest revokeRefreshTokenRequest);
    Task<ApiResponse<RevokeRefreshTokenResponse>> RevokeRefreshTokens(RevokeRefreshTokenRequest revokeRefreshTokenRequest);
    
    Task<ApiResponse<RefreshTokenResponse>> TryRefreshTokenAsync();
}
