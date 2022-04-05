
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;


namespace SecuredAPI.EndPoints.Authenication;
public class Logout : EndpointBaseAsync.WithoutRequest.WithoutResult
{
    private readonly IAuthenticationService _authenticationService;

    public Logout(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost("/authentication/logout")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
        
        return Ok();
    }
}
