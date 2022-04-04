namespace Security.Shared.Models.Authentication;

public class LoginRequestDto
{
    public const string Route = "api/authentication/login";
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool RememberMe { get; set; }
}
