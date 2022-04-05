using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;
using Security.Shared.Permissions;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class Create : EndpointBaseAsync
                        .WithRequest<CreateRoleRequest>
                        .WithActionResult<CreateRoleResponse>
{
    private readonly IRoleService _roleService;

    public Create(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(CreateRoleRequest.Route)]
    [HasPermission(Permission.RoleCreate)]
    public override async Task<ActionResult<CreateRoleResponse>> HandleAsync([FromBody] CreateRoleRequest createRoleRequest, CancellationToken cancellationToken = default)
    {
        CreateRoleResponse createRoleResponse = await _roleService.Create(createRoleRequest);
        return this.ToActionResult<CreateRoleResponse>(createRoleResponse);
    }
}
