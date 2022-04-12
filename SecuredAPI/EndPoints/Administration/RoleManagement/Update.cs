using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;

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
        
      UpdateRoleResponse response = await _roleService.UpdateAsync(editRoleRequest);
      return this.ToActionResult<UpdateRoleResponse>(response);
        
    }
}
