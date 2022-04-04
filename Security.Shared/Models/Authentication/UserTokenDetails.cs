using Security.Shared.Permissions.Enums;

using System.Security.Claims;

namespace Security.Shared.Models.Authentication;

public class UserTokenDetails 
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Permissions { get; set; } = null!;

}

