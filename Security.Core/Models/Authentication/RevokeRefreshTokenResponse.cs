namespace Security.Core.Models.Authentication;

public class RevokeRefreshTokenResponse
{
    public Guid UserId { get; set; }
    public string? ErrorMessage { get; set; }    

}
