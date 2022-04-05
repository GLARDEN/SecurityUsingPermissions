using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Data;
using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;
using Security.Shared.Permissions;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class Delete : EndpointBaseAsync
                        .WithRequest<DeleteRoleRequest>
                        .WithActionResult<DeleteRoleResponse>
{
    private readonly IRoleService _roleService;
    
    public Delete(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Deletes a specified forecast
    /// </summary>
    [HttpPost(DeleteRoleRequest.Route)]
    [HasPermission(Permission.RoleDelete)]
    public override async Task<ActionResult<DeleteRoleResponse>> HandleAsync([FromBody] DeleteRoleRequest request, CancellationToken cancellationToken = default)
    {
        DeleteRoleResponse deleteRoleResponse = await _roleService.Delete(request);
        return this.ToActionResult<DeleteRoleResponse>(deleteRoleResponse);
    }
}
