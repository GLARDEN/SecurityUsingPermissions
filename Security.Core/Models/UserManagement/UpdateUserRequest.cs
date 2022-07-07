using Security.Core.Models.Authentication;

namespace Security.Core.Models.UserManagement;

public class UpdateUserRequest
{
    public const string Route = "api/administration/userManagement/Update";
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public IEnumerable<UserRoleDto> Roles { get; set; } = null!;

    public IEnumerable<RefreshTokenDto> RefreshTokens { get; set; } = null!;


}

