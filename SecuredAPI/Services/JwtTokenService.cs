
using Security.Core.Models.Authentication;
using Security.Core.Permissions.Constants;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecuredAPI.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IEnumerable<Claim> GetClaims(UserTokenDetails userTokenDetails)
    {     
        var authenticationClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userTokenDetails.Id.ToString()),
            new Claim(ClaimTypes.Name, userTokenDetails.Email),
            new Claim(PermissionConstants.PackedPermissionClaimType,userTokenDetails.Permissions ?? "")
        };
        return authenticationClaims;
    }

    public string GenerateToken(UserTokenDetails userTokenDetails)
    {
        try
        {
            IEnumerable<Claim> claims = GetClaims(userTokenDetails);

            // Get secret key
            var keyData = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyData);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            DateTime expireTime = DateTime.UtcNow.AddMinutes(10);

            var jwtSecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: expireTime,
                signingCredentials: signingCredentials
             );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string? ValidateToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JWTSettings:Secret"]);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "role"
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value.ToString();

            // Checks the exp field of the token
            var expiry = jwtToken.Claims.First(claim => claim.Type.Equals("exp"));
            if (expiry == null)
                return null;

            // The exp field is in Unix time
            var expiryDatetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
            var currentDateTime = DateTime.UtcNow;
            if (expiryDatetime.UtcDateTime <= currentDateTime)
            {
                return null;
            }
            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }

}
