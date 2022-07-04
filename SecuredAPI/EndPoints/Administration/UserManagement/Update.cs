using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Services;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;

namespace SecuredAPI.EndPoints.Administration.UserManagement;

public class Update : EndpointBaseAsync
                        .WithRequest<UpdateUserRequest>
                        .WithActionResult<UpdateUserResponse>
{
    private readonly IUserManagementService _userManagementService;

    public Update(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost(UpdateUserRequest.Route)]
    [HasPermission(Permission.UserEdit)]
    public override async Task<ActionResult<UpdateUserResponse>> HandleAsync([FromBody] UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
 
        UpdateUserResponse response = await _userManagementService.UpdateAsync(updateUserRequest);

        return this.ToActionResult<UpdateUserResponse>(response);


    }
}
