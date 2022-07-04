using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Administration.RoleManagement.Services;

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
        return this.ToActionResult<ListRolesResponse>(await _roleService.ListAsync());
    }
}
