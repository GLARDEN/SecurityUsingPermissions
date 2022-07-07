using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using System.Security.Cryptography;
using System.Text;

namespace Security.Core.Models.Authentication;

public class RefreshToken
{
    public Guid UserId { get; private set; }
    public Guid DeviceId { get; private set; }
    public string Token { get; private set; } = null!;
    public byte[] TokenHash { get; private set; } = null!;
    public byte[] TokenSalt { get; private set; } = null!;
    public DateTime Expiry { get; private set; }
    public DateTime Created { get; private set; }

    /// <summary>
    /// If this is false, then you should not renew the JWT token
    /// Can manually set to true to force a new login
    /// </summary>
    public bool IsValid { get; private set; }

    public RefreshToken(Guid userId, Guid deviceId, DateTime created,DateTime expiry,string token, byte[] tokenHash, byte[] tokenSalt)
    {
        UserId = userId;
        DeviceId = deviceId;
        Created = created;
        Expiry = expiry;
        Token = token;
        TokenHash = tokenHash;
        TokenSalt = tokenSalt;
        IsValid = true;
    }

    public static string GenerateTokenValue()
    {
        //see https://www.blinkingcaret.com/2018/05/30/refresh-tokens-in-asp-net-core-web-api/
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        string tokenValue = Convert.ToBase64String(randomNumber);
        return tokenValue;
    }   
    /// <summary>
    /// Use this if a) RefreshToken has been used, or b) you want to stop the user from being able refresh their token
    /// </summary>
    public void MarkAsInvalid()
    {
        IsValid = false;
    }
}
