using Microsoft.AspNetCore.Authorization;
using Security.Shared.Authorization.Requirements;
using Security.Shared.Permissions.Constants;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions.Helpers;

namespace Security.Shared.Authorization.Handlers;

public class PermissionPolicyHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly Type _enumPermissionType = typeof(Permission);    

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var permissionsClaim =
            context.User.Claims.SingleOrDefault(c => c.Type == PermissionConstants.PackedPermissionClaimType);
        // If user does not have the scope claim, get out of here
        if (permissionsClaim == null)
            return Task.CompletedTask;

        if (_enumPermissionType.ThisPermissionIsAllowed(permissionsClaim.Value, requirement.PermissionName))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}