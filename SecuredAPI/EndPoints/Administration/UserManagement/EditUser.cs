using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SecuredAPI.Services;

using Security.Shared.Models;
using Security.Shared.Models.Administration.Role;
using Security.Shared.Permissions;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class EditUser : EndpointBaseAsync
                        .WithRequest<EditUserRequest>
                        .WithActionResult<EditUserResponse>
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
    public override async Task<ActionResult<EditUserResponse>> HandleAsync([FromBody] EditUserRequest updateUserRequest, CancellationToken cancellationToken = default)
    {
        if (updateUserRequest == null || !ModelState.IsValid)
        {
            return new EditUserResponse() { Success = false, ErrorMessage = "Update User Request is null." };
        }

        EditUserResponse updateUserResponse = await _userManagementService.UpdateUser(updateUserRequest);

        if (!updateUserResponse.Success)
        {
            return BadRequest(updateUserResponse);
        }

        return updateUserResponse;
    }
}
