using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Data;
using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class Delete : EndpointBaseAsync
                        .WithRequest<DeleteRoleRequest>
                        .WithActionResult<DeleteRoleResponse>
{
    private readonly IRoleService _roleService;
    private readonly AppDbContext _appDbContext;

    public Delete(IRoleService roleService)
    {
        _roleService = roleService;

    }

    /// <summary>
    /// Deletes a specified forecast
    /// </summary>
    [HttpPost("api/administration/role/delete")]

    public override async Task<ActionResult<DeleteRoleResponse>> HandleAsync([FromBody] DeleteRoleRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _roleService.Delete(request);

        return result;
    }


}
