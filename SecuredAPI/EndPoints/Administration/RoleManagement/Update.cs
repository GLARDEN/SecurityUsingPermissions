using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Shared.Models.Administration.Role;
using Security.Shared.Models.Administration.RoleManagement;
using Security.Shared.Permissions;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class Update : EndpointBaseAsync
                        .WithRequest<UpdateRoleRequest>
                        .WithActionResult<UpdateRoleResponse>
{
    private readonly IRoleService _roleService;

    public Update(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(UpdateRoleRequest.Route)]
    [HasPermission(Permission.RoleEdit)]
    public override async Task<ActionResult<UpdateRoleResponse>> HandleAsync([FromBody] UpdateRoleRequest editRoleRequest, CancellationToken cancellationToken = default)
    {
        {
            UpdateRoleResponse response = await _roleService.UpdateAsync(editRoleRequest);

            return this.ToActionResult<UpdateRoleResponse>(response);

        }
}
}
