using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Administration.RoleManagement.Services;
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
    /// Creates a new Role
    /// </summary>
    
    [HttpPost(CreateRoleRequest.Route)]
    [HasPermission(Permission.RoleCreate)]
    public override async Task<ActionResult<CreateRoleResponse>> HandleAsync([FromBody] CreateRoleRequest createRoleRequest,
                                                                             CancellationToken cancellationToken = default)
    {        
               
       return (await _roleService.CreateAsync(createRoleRequest)).ToActionResult(this);
    }
}
