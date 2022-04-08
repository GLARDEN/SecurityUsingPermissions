using Security.Shared.Models.UserManagement;

namespace BlazorClient.Services;

public interface IPermissionDisplayService
{
    List<PermissionGroupDto> GroupPermissionsForDisplay(bool excludeFilteredPermissions=false);
}
