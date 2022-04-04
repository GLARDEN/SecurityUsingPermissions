using Security.Shared.Permissions.Enums;

namespace Security.Shared.Models.Administration.RoleManagement;

public class RoleDto
{
    public string RoleName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string PermissionsInRole { get; set; } = null!;

    public RoleDto() { } //required by EF Core


}