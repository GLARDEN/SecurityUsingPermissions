using Security.Shared.Models.UserManagement;

namespace BlazorClient.Services;

public interface IPermissionDisplayService
{
    List<RoleDisplayDto> GetRolesForDisplay(bool excludeFilteredPermissions=false);
}
