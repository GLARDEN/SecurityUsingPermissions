// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Security.Shared.Permissions.Enums;

namespace Security.Shared.Permissions.Extensions;

public static class PermissionUnpacker
{
    public static List<string> ConvertPackedPermissionToNames(this string packedPermissions)
    {
        if (packedPermissions == null)
            return null;

        var permissionNames = new List<string>();
        foreach (var permissionChar in packedPermissions)
        {
            var enumName = Enum.GetName(typeof(Permission), (ushort)permissionChar);
            if (enumName != null)
                permissionNames.Add(enumName);
        }

        return permissionNames;
    }
}
