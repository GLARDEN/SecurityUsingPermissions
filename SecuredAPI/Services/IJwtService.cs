
using Security.Shared.Models.Authentication;

using System.Security.Claims;

namespace SecuredAPI.JwtHelpers;

public interface IJwtTokenService
{
    string GenerateToken(UserTokenDetails userTokenDetails);
    IEnumerable<Claim> GetClaims(UserTokenDetails userTokenDetails);
    string? ValidateToken(string token);
}