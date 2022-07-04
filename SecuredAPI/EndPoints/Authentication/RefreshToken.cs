
using Ardalis.ApiEndpoints;

using Microsoft.AspNetCore.Mvc;

using Ardalis.Result.AspNetCore;

using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Specifications;
using Ardalis.Result;

namespace SecuredAPI.EndPoints.Authentication;

public class RefreshToken : EndpointBaseAsync
                            .WithRequest<RefreshTokenRequest>
                            .WithActionResult<RefreshTokenResponse>

{
    private readonly IAuthenticationService _authenticationService;     

    public RefreshToken(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(RefreshTokenRequest.Route)]
    [AllowAnonymous]
    public override async Task<ActionResult<RefreshTokenResponse>> HandleAsync([FromBody] RefreshTokenRequest refreshTokenRequest, 
                                                                                          CancellationToken cancellationToken = default)
    {  
            var result = await _authenticationService.RefreshTokenAsync(refreshTokenRequest);

            if (result.Status == ResultStatus.Ok)
            {  
                return this.ToActionResult(Result<RefreshTokenResponse>.Success(result.Value));
            }
        
        return this.ToActionResult(Result<RefreshTokenResponse>.Unauthorized());
    }
}
