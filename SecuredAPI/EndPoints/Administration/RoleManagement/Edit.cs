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

public class Edit : EndpointBaseAsync
                        .WithRequest<EditRoleRequest>
                        .WithActionResult<EditRoleResponse>
{
    private readonly IRoleService _roleService;

    public Edit(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(EditRoleRequest.Route)]
    [HasPermission(Permission.RoleEdit)]
    public override async Task<ActionResult<EditRoleResponse>> HandleAsync([FromBody] EditRoleRequest editRoleRequest, CancellationToken cancellationToken = default)
    {
        {
            EditRoleResponse response = await _roleService.EditAsync(editRoleRequest);

            return this.ToActionResult<EditRoleResponse>(response);

        }
}
}
