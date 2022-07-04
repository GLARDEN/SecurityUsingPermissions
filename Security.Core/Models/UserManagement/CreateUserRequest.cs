namespace Security.Core.Models.UserManagement;

public class CreateUserRequest
{
    public const string Route = "api/Administration/UserManagement/Create";

    public string Email { get; set; } = null!;

    public string TemporaryPassword { get; set; } = null!;

    public List<UserRoleDto> AssignedRoles { get; set; } = new List<UserRoleDto>();
    
    public bool Enabled { get; set; }

    public CreateUserRequest()
    {

    }
}
