using Ardalis.Result;

using SecuredAPI.EndPoints.Authenication;

using Security.Core.Models.Authentication;

namespace SecuredAPI.Services;

public interface IRefreshTokenService
{
    RefreshToken GenerateRefreshToken(UserTokenDetails userTokenDetails);
    bool ValidateRefreshToken(string token, byte[] tokenHash, byte[] tokenSalt);

    Task<Result<RevokeRefreshTokenResponse>> RevokeUserRefreshTokenAsync(RevokeRefreshTokenRequest revokeRefreshTokenRequest);
    Task<Result<RevokeRefreshTokenResponse>> RevokeUserRefreshTokensAsync(RevokeRefreshTokenRequest revokeRefreshTokenRequest);
}
