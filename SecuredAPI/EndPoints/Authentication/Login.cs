
using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Ardalis.Result.AspNetCore;

using Security.Core.Models.Authentication;
using Security.Core.Services;
using Ardalis.Result;
using Security.Core.Models.Administration.RoleManagement;

namespace SecuredAPI.EndPoints.Authenication;

public class Login : EndpointBaseAsync
                            .WithRequest<LoginRequest>
                            .WithActionResult<LoginResponse>
{
    private readonly IAuthenticationService _authenticatiService;     

    public Login(IAuthenticationService authenticationService)
    {
        _authenticatiService = authenticationService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(LoginRequest.Route)]
    [AllowAnonymous]
    public override async Task<ActionResult<LoginResponse>> HandleAsync([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
         return (await _authenticatiService.LoginAsync(loginRequest)).ToActionResult(this);        
    }
}
