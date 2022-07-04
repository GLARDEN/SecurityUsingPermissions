namespace Security.Core.Models.Authentication;

public class RefreshTokenResponse
{
    public Guid UserId { get; set; }
    
    public string? ErrorMessage { get; set; }
    public string? JWTToken { get; set; }
    public string? RefreshToken { get; set; }
    public bool IsAuthenticationSuccessful { get; set; }

}
