
using Security.Core.Models;

namespace Security.Core.Permissions.Services;

public interface IPermissionService
{
    List<PermissionGroupDto> GroupPermissionsForDisplay(bool excludeFilteredPermissions = false);
    List<string> ValidatePermissionName(IEnumerable<string> permissionNames);
}
