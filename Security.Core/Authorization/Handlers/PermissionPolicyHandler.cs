using Microsoft.AspNetCore.Authorization;

using Security.Core.Authorization.Requirements;
using Security.Core.Permissions.Constants;
using Security.Core.Permissions.Enums;
using Security.Core.Permissions.Helpers;

namespace Security.Core.Authorization.Handlers;

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