using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.UserManagement;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class EditUser : EndpointBaseAsync
                        .WithRequest<UpdateUserRequest>
                        .WithActionResult<UpdateUserResponse>
{
    private readonly IUserManagementService _userManagementService;

    public EditUser(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost("api/administration/usermanagement/editUser")]
    [HasPermission(Permission.UserEdit)]
    public override async Task<ActionResult<UpdateUserResponse>> HandleAsync([FromBody] UpdateUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
        //if (updateUserRequest == null || !ModelState.IsValid)
        //{
        //    return new UpdateUserResponse() { Success = false, ErrorMessage = "Update User Request is null." };
        //}

        UpdateUserResponse response = await _userManagementService.UpdateUserAsync(updateUserRequest);
                
        return this.ToActionResult<UpdateUserResponse>(response);


    }
}
