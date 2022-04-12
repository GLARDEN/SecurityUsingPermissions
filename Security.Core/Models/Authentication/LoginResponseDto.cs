namespace Security.Core.Models.Authentication;

public class LoginResponseDto
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string ErrorMessage { get; set; }
    public string Token { get; set; }
    public bool IsAuthenticationSuccessful { get; set; }

}

