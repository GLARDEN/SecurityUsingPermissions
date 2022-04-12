
using Security.Core.Models;

namespace Security.Core.Permissions.Services;

public interface IPermissionDisplayService
{
    List<PermissionGroupDto> GroupPermissionsForDisplay(bool excludeFilteredPermissions = false);
}
