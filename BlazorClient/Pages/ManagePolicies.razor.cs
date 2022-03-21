using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions;

namespace BlazorClient.Pages
{
    [HasPermission(Permission.UserAdmin)]
    public partial class ManagePolicies
    {
    }
}