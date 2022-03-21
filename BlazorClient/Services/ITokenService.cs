using System.Security.Claims;

namespace BlazorClient.Services;

public interface ITokenService
{
    Task<IEnumerable<Claim>> GetClaimsFromTokenAsync(string? token);
    Task<bool> IsTokenExpiredAsync(string? token);  
    Task<string> GetTokenAsync();
    Task SetTokenAsync(string token);
    Task RemoveTokenAsync();
}