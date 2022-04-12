namespace Security.Core.Models.UserManagement;

public class UpdateUserRequest
{
    public const string Route = "administration/usermanagement/update";
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public IEnumerable<UserRoleDto> Roles { get; set; }
}

