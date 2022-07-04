using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.Administration.RoleManagement;
using Security.Core.Models.Administration.RoleManagement.Services;
using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Services;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;

namespace SecuredAPI.EndPoints.Administration.UserManagement;

public class Create : EndpointBaseAsync
                        .WithRequest<CreateUserRequest>
                        .WithActionResult<CreateUserResponse>
{
    private readonly IUserManagementService _userManagementService;

    public Create(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    /// <summary>
    /// Creates a new Role
    /// </summary>

    [HttpPost(CreateUserRequest.Route)]
    [HasPermission(Permission.UserAdd)]
    public override async Task<ActionResult<CreateUserResponse>> HandleAsync([FromBody] CreateUserRequest createRoleRequest,
                                                                             CancellationToken cancellationToken = default)
    {

        return (await _userManagementService.CreateAsync(createRoleRequest)).ToActionResult(this);
    }
}
