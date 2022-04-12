namespace Security.Core.Models.Administration.RoleManagement;

public class UpdateRoleRequest
{
    public const string Route = "api/administration/roleManagement/update";
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public IEnumerable<string> PermissionNames { get; set; } = null!;

    public static UpdateRoleRequest FromDto(RoleDto roleDto)
    {
        return new()
        {
            Id = roleDto.Id,
            Name = roleDto.Name,
            Description = roleDto.Description,
            PermissionNames = roleDto.PermissionsInRole
        };
    }
}

