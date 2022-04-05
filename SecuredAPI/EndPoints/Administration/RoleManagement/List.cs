using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

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
    [HttpGet(ListRolesRequest.Route)]
    public override async Task<ActionResult<ListRolesResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        ListRolesResponse response = await _roleService.ListAsync();

        return this.ToActionResult<ListRolesResponse>(response);
    }
}
