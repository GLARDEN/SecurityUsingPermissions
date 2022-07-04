namespace Security.Core.Models.Authentication;

public class LogOutResponse
{
    public Guid UserId { get; set; }    
    public string? ErrorMessage { get; set; }
    public bool LogOutSuccess { get; set; }
}
