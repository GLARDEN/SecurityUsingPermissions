
using Ardalis.ApiEndpoints;

using AutoMapper;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.JwtHelpers;
using SecuredAPI.Services;

using Security.Shared.Models.Authentication;

namespace SecuredAPI.EndPoints.Authenication;

public class Login : EndpointBaseAsync
    .WithRequest<LoginRequestDto>
    .WithActionResult<LoginResponseDto>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IMapper _mapper;
    //private readonly UserManager<ApplicationUser> _userManager;
    //private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IJwtTokenService _jwtService;

    public Login(IAuthenticationService authenticationService, IMapper mapper, IConfiguration configuration, IJwtTokenService jwtService)
    {
        _authenticationService = authenticationService;
        _mapper = mapper;
        //_userManager = userManager;
        //_signInManager = signInManager;
        _configuration = configuration;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost("api/authentication/login")]
    [AllowAnonymous]
    public override async Task<ActionResult<LoginResponseDto>> HandleAsync([FromBody] LoginRequestDto loginRequest, CancellationToken cancellationToken = default)
    {
        LoginResponseDto loginResponse = await _authenticationService.LoginAsync(loginRequest);            
        if (loginResponse.IsAuthenticationSuccessful)
        {   
            return loginResponse;
        }
        else
        {
            return Unauthorized(new LoginResponseDto { ErrorMessage = "Log attempt failed" });
        }
    }
}
