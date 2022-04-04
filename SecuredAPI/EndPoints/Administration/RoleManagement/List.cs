using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class List : EndpointBaseAsync
                        .WithoutRequest
                        .WithActionResult<ListRolesResponse>
{
    private readonly IRoleService _roleService;

    public List(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpGet("api/administration/role/list")]
    public override async Task<ActionResult<ListRolesResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        ListRolesResponse listRolesResponse = await _roleService.GetRolesAsync();

        if (!listRolesResponse.Success)
        {
            return BadRequest(listRolesResponse);
        }

        return listRolesResponse;
    }
}
