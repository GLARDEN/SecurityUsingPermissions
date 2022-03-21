using System;

using Microsoft.AspNetCore.Authorization;

using Security.Shared.Permissions.Enums;

namespace Security.Shared.Permissions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) : base(permission.ToString())
    { }
}
