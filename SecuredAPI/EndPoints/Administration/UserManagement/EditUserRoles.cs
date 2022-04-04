using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Security.Shared.Models;

namespace SecuredAPI.EndPoints.Administration.RoleManagement;

public class EditUserRoles : EndpointBaseAsync
                        .WithRequest<EditUserRolesRequest>
                        .WithActionResult<EditUserRolesResponse>
{
    private readonly IUserManagementService _userManagementService;

    public EditUserRoles(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    /// <summary>
    /// Authenticates and logs user in
    /// </summary>
    [HttpPost("api/administration/usermanagement/editUserRoles")]
    
    public override async Task<ActionResult<EditUserRolesResponse>> HandleAsync([FromBody] EditUserRolesRequest request, CancellationToken cancellationToken = default)
    {
        if (request == null || !ModelState.IsValid)
        {
            return new EditUserRolesResponse() { Success = false, ErrorMessage = "Update User Request is null." };
        }

        EditUserRolesResponse response = await _userManagementService.EditUserRoles(request);

        if (!response.Success)
        {
            return BadRequest(response);
        }

        return response;
    }
}
