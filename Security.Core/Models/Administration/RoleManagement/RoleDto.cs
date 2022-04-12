
namespace Security.Core.Models.Administration.RoleManagement;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<string> PermissionsInRole { get; set; } = null!;

    public RoleDto() { } //required by EF Core

}