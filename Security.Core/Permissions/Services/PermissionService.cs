
using Security.Core.Models;
using Security.Core.Permissions.Enums;

using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Security.Core.Permissions.Services;

public class PermissionService : IPermissionService
{
    private readonly Type _permissionType = typeof(Permission);

    /// <summary>
    /// This returns all the enum permission names with the various display attribute data
    /// NOTE: It does not show enum names that
    /// a) don't have an <see cref="DisplayAttribute"/> on them. They are assumed to not 
    /// b) Which have a <see cref="ObsoleteAttribute"/> applied to that name
    /// </summary>
    /// <param name="enumType">type of the enum permissions</param>
    /// <param name="excludeFilteredPermissions">if trie then it won't show permissions where the AutoGenerateFilter is true</param>
    /// <returns>a list of PermissionDisplay classes containing the data</returns>
    public List<PermissionGroupDto> GroupPermissionsForDisplay(bool excludeFilteredPermissions = false)
    {
        List<PermissionGroupDto> result = new();

        string groupName = string.Empty;
        string currentGroupName = string.Empty;

        PermissionGroupDto permissionGroup = null;

        foreach (var permissionName in Enum.GetNames(_permissionType))
        {
            var member = _permissionType.GetMember(permissionName);

            //This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
            var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
            if (obsoleteAttribute != null)
                continue;

            //If there is no DisplayAttribute then the Enum is not used
            var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute == null)
                continue;

            //remove permissions where AutoGenerateFilter is true
            if (excludeFilteredPermissions && displayAttribute.GetAutoGenerateFilter() == true)
                continue;

            groupName = displayAttribute.GroupName ?? "";

            if (groupName != currentGroupName)
            {
                currentGroupName = groupName;
                permissionGroup = new PermissionGroupDto() { GroupName = currentGroupName, Permissions = new() };
                result.Add(permissionGroup);
            }

            PermissionInfoDto permissionInfo = new PermissionInfoDto()
            {
                ShortName = displayAttribute.Name,
                Description = displayAttribute.Description,
                PermissionName = permissionName,
                IsSelected = false
            };

            permissionGroup?.Permissions.Add(permissionInfo);
        }

        return result;
    }


    public List<string> ValidatePermissionName(IEnumerable<string> permissionNames)
    {
        List<string> invalidPermissionNames = new();
        foreach(string permissionName in permissionNames)
        {
            var isValid = Enum.TryParse<Permission>(permissionName,out Permission foundPermission);
            if (!isValid)
            {
                invalidPermissionNames.Add(permissionName);
            }
        }

        return invalidPermissionNames;
    }
}
