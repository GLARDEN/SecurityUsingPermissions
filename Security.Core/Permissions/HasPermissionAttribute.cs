using System;

using Microsoft.AspNetCore.Authorization;

using Security.Core.Permissions.Enums;

namespace Security.Core.Permissions;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission) : base(permission.ToString())
    { }
}
