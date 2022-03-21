using Blazored.LocalStorage;

using System.Security.Claims;
using System.Text.Json;

namespace BlazorClient.Services;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorageService;

    public TokenService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public async Task<IEnumerable<Claim>> GetClaimsFromTokenAsync(string? token)
    {
       
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var jwtClaims = ParseClaimsFromJwt(token);
        return jwtClaims;
    }

    public async Task<bool> IsTokenExpiredAsync(string? token)
    {
        bool expire = false;
        
        if (string.IsNullOrWhiteSpace(token))
        {
            expire = true;
            return expire;
        }

        var jwtClaims = ParseClaimsFromJwt(token);

        if (token != null)
        {
            var expiry = jwtClaims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
            // The exp field is in Unix time
            var expiryDatetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
            var currentDateTime = DateTime.UtcNow;
            if (expiryDatetime.UtcDateTime <= currentDateTime)
            {
                await _localStorageService.RemoveItemAsync("authenticationToken");
                expire = true;
            }
        }
        else
        {
            expire = true;
        }

        return expire;
    }

    public async Task<string> GetTokenAsync()
    {
        var token = await _localStorageService.GetItemAsync<string>("authenticationToken");
        return token ?? "";
    }

    public async Task SetTokenAsync(string token)
    {
        await _localStorageService.SetItemAsync("authenticationToken", token);
    }
    public async Task RemoveTokenAsync()
    {
        await _localStorageService.RemoveItemAsync("authenticationToken");
    }
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {        
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);

        Dictionary<string,object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        IEnumerable<Claim> claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

        return claims;
    }
    
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

}
