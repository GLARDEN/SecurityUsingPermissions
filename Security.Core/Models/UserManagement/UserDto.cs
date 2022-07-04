using Security.Core.Models.Authentication;

namespace Security.Core.Models.UserManagement;
public class UserDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public DateTime CreatedWhen { get; set; } = DateTime.Now;

    public List<UserRoleDto> AssignedRoles { get; set; } = new List<UserRoleDto>();
    
    public List<RefreshTokenDto> RefreshTokens { get; set; } = new List<RefreshTokenDto>();
    public UserDto() { }

}
