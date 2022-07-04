using System.Security.Claims;

namespace Security.Core.Models.Authentication;

public class LoginResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string? ErrorMessage { get; set; }
    public string? JwtToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool IsAuthenticationSuccessful { get; set; }

}
