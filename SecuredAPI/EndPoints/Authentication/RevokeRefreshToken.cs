
using Ardalis.ApiEndpoints;

using Microsoft.AspNetCore.Mvc;

using Ardalis.Result.AspNetCore;

using Security.Core.Models.Authentication;
using Ardalis.Result;

namespace SecuredAPI.EndPoints.Authentication;

public class RevokeRefreshToken : EndpointBaseAsync
                            .WithRequest<RevokeRefreshTokenRequest>
                            .WithActionResult<RevokeRefreshTokenResponse>

{
    private readonly IRefreshTokenService _refreshTokenService;

    public RevokeRefreshToken(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(RevokeRefreshTokenRequest.Route)]
    [AllowAnonymous]
    public override async Task<ActionResult<RevokeRefreshTokenResponse>> HandleAsync([FromBody] RevokeRefreshTokenRequest revokeRefreshTokenRequest, CancellationToken cancellationToken = default)
    {
        Result<RevokeRefreshTokenResponse> response;
        if (revokeRefreshTokenRequest.RevokeAll)
        {
            response = await _refreshTokenService.RevokeUserRefreshTokensAsync(revokeRefreshTokenRequest);
        }
        else
        {
            response = await _refreshTokenService.RevokeUserRefreshTokenAsync(revokeRefreshTokenRequest);
        }
        

        if (response.Status == ResultStatus.Ok)
        {
            return this.ToActionResult(Result<RevokeRefreshTokenResponse>.Success(response.Value));
        }

        return this.ToActionResult(Result<RevokeRefreshTokenResponse>.Unauthorized());
    }
}