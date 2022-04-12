using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Administration.RoleManagement;

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
        ListRolesResponse response = await _roleService.ListAsync();

        return this.ToActionResult<ListRolesResponse>(response);
    }
}
