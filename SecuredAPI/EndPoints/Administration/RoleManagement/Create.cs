using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;

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
