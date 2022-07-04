using Security.Core.Models.Authentication;

using System.Security.Claims;

namespace SecuredAPI.Services;

public interface IJwtTokenService
{
    string GenerateJWTToken(UserTokenDetails userTokenDetails);
    IEnumerable<Claim> GetClaims(UserTokenDetails userTokenDetails);
    SigningCredentials GetSigningCredentials();
    bool ValidateJwtToken(string? token);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
}