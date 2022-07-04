namespace Security.Core.Models.UserManagement;
public class UserRoleDto
{
    public Guid UserId { get; set; }
    public string RoleName { get; set; }
    public IEnumerable<string> AssignedPermissions { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public UserRoleDto() { }//Required by EF Core

}
