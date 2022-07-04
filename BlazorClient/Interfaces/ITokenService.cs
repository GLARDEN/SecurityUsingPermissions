
using System.Security.Claims;

namespace BlazorClient.Interfaces;

public interface IJwtTokenService
{
    IEnumerable<Claim> GetClaimsFromJwtToken(string? token);
    Task<bool> IsJwtTokenExpiredAsync(string? token);
    Task<string> GetJwtTokenAsync();
    Task<double> GetMinutesUntilTokenExpiresAsync(string? token);
    Task SetJwtTokenAsync(string token);   
    Task RemoveJwtTokenAsync();
   
    
}