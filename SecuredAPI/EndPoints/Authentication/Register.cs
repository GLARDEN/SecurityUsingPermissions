using Ardalis.ApiEndpoints;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Authentication;

namespace SecuredAPI.EndPoints.Authenication;
[AllowAnonymous]
public class Register : EndpointBaseAsync.WithRequest<RegistrationRequestDto>.WithoutResult
{
    private readonly IAuthenticationService _authenticationService;

    public Register(IAuthenticationService authenticationService) {
        _authenticationService = authenticationService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost("api/authentication/register")]    
    public override async Task<ActionResult> HandleAsync([FromBody] RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken = default)
    {

        if (registrationRequestDto == null || !ModelState.IsValid)
            return BadRequest();

        var result = await _authenticationService.RegisterAsync(registrationRequestDto, registrationRequestDto.Password);
        if (!result.IsRegistrationSuccessful)
        {   return BadRequest(result); 
        }

        return StatusCode(201);
    }
}
