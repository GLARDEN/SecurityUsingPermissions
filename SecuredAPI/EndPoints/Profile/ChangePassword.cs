
using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models;

using System.Security.Claims;

namespace SecuredAPI.EndPoints.Profile;

public class ChangePassword : EndpointBaseAsync
    .WithRequest<ChangePasswordRequest>
    .WithActionResult<ChangePasswordResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IJwtTokenService _jwtService;

    public ChangePassword(IAuthenticationService authenticationService, IMapper mapper, IConfiguration configuration, IJwtTokenService jwtService)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
        _configuration = configuration;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Changes password for the authenticated user
    /// </summary>
    [HttpPost("api/authentication/changepassword")]
    [Authorize]
    public override async Task<ActionResult<ChangePasswordResponse>> HandleAsync([FromBody] ChangePasswordRequest changePasswordRequest, CancellationToken cancellationToken = default)
    {
        string userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        changePasswordRequest.UserId = Guid.Parse(userIdClaim);
    
        ChangePasswordResponse changePasswordResponse = await _authenticationService.ChangePassword(changePasswordRequest);
        if (!changePasswordResponse.PasswordChangeSuccessfull)
        {
            return BadRequest(changePasswordResponse);
        }
        else
        {
            return Ok(changePasswordResponse);
        }
    }
}
