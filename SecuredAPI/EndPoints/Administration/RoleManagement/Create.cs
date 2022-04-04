using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;

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
    [HttpPost("api/administration/role/create")]
    public override async Task<ActionResult<CreateRoleResponse>> HandleAsync([FromBody] CreateRoleRequest createRoleRequest, CancellationToken cancellationToken = default)
    {
        if (createRoleRequest == null || !ModelState.IsValid)
        {
            return new CreateRoleResponse() { Success = false, ErrorMessage = "Create Role Request is null." };
        }

        CreateRoleResponse createRoleResponse = await _roleService.Create(createRoleRequest);

        if (!createRoleResponse.Success)
        {
            return BadRequest(createRoleResponse);
        }

        return createRoleResponse;
    }
}
