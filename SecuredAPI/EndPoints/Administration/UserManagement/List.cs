using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Shared.Models;

namespace SecuredAPI.EndPoints.Administration.UserManagement;

public class List : EndpointBaseAsync
                        .WithoutRequest
                        .WithActionResult<ListUsersResponse>
{
    private readonly IUserManagementService _userManagementService;

    public List(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpGet(ListUsersRequest.Route)]

    public override async Task<ActionResult<ListUsersResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        ListUsersResponse response = await _userManagementService.ListAsync();
                
        return this.ToActionResult<ListUsersResponse>(response);
    }
}