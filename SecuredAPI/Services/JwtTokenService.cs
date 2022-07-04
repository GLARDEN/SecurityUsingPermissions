using Security.Core.Models.Authentication;
using Security.Core.Permissions.Constants;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            new Claim(ClaimsConstants.DeviceId, userTokenDetails.DeviceId.ToString()),
            new Claim(PermissionConstants.PackedPermissionClaimType,userTokenDetails.Permissions ?? "")
        };
        return authenticationClaims;
    }
    public SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha512Signature);
    }
    public string GenerateJWTToken(UserTokenDetails userTokenDetails)
    {
        try
        {
            IEnumerable<Claim> claims = GetClaims(userTokenDetails);

            // Get secret key
            var keyData = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyData);

            SigningCredentials signingCredentials = GetSigningCredentials();

            DateTime expireTime = DateTime.UtcNow.AddMinutes(5);

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
    public bool ValidateJwtToken(string? token)
    {
        bool tokenIsValid = false;
        if (token == null)
            return tokenIsValid;

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
                ValidateLifetime = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero,
                RoleClaimType = "role"
            }, out SecurityToken validatedToken); ;

            var jwtToken = (JwtSecurityToken)validatedToken;

            // Checks the exp field of the token
            var expiry = jwtToken.Claims.First(claim => claim.Type.Equals("exp"));

            if (expiry == null)
                return tokenIsValid;
           
            tokenIsValid = true;

            return tokenIsValid;
        }
        catch
        {
            // return null if validation fails
            return tokenIsValid;
        }
    }
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
    {
        if (!ValidateJwtToken(token))
        {
            throw new SecurityTokenException("Invalid token");
        }

        // Get secret key
        var keyData = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value);

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyData);

        SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey,
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }


}
