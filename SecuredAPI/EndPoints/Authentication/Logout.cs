
using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Security.Data;

namespace SecuredAPI.EndPoints.Authenication;
public class Logout : EndpointBaseAsync.WithoutRequest.WithoutResult
{
    private readonly IMapper _mapper;

    public Logout(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost("/authentication/logout")]
    public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = default)
    {
//        await _signInManager.SignOutAsync();
        return Ok();
    }
}
