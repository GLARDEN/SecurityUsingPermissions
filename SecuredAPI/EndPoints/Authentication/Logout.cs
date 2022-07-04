
using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Authentication;

namespace SecuredAPI.EndPoints.Authenication;
public class Logout : EndpointBaseAsync
                            .WithRequest<LogOutRequest>
                            .WithActionResult<LogOutResponse>
{
    private readonly IAuthenticationService _authenticationService;

    public Logout(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(LogOutRequest.Route)]
    [AllowAnonymous]
    public override async Task<ActionResult<LogOutResponse>> HandleAsync([FromBody] LogOutRequest logOutRequest, CancellationToken cancellationToken = default)
    {        

         return (await _authenticationService.LogOutAsync(logOutRequest)).ToActionResult(this);
    }
}
