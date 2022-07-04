using BlazorClient.Interfaces;

using Blazored.LocalStorage;

using Security.Core.Models.UserManagement;

using System.Security.Claims;
using System.Text.Json;

namespace BlazorClient.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly ILocalStorageService _localStorageService;

    public JwtTokenService(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public IEnumerable<Claim> GetClaimsFromJwtToken(string? token)
    {
       
        if (string.IsNullOrWhiteSpace(token))
        {
            return null;
        }

        var jwtClaims = ParseClaimsFromJwt(token);
        return jwtClaims;
    }

    public async Task<bool> IsJwtTokenExpiredAsync(string? token)
    {
        bool expire = false;
        
        if (string.IsNullOrWhiteSpace(token))
        {
            expire = true;
            return expire;
        }

        if (token != null)
        {
            var jwtClaims = ParseClaimsFromJwt(token);
            var expiry = jwtClaims.Where(claim => claim.Type.Equals("exp")).FirstOrDefault();
            // The exp field is in Unix time
            var expiryDatetime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expiry.Value));
            var currentDateTime = DateTime.UtcNow;
            if (expiryDatetime.UtcDateTime <= currentDateTime)
            {              
                expire = true;
            }
        }
        else
        {
            expire = true;
        }
        return expire;
    }

    public async Task<double> GetMinutesUntilTokenExpiresAsync(string? jwtToken)
    {
        if(String.IsNullOrEmpty(jwtToken))
        {
            return 0;
        }

        IEnumerable<Claim> jwtClaims = GetClaimsFromJwtToken(jwtToken);

        var expiry = jwtClaims.FirstOrDefault(c => c.Type.Equals("exp"))?.Value; 

        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(expiry));

        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;
        return diff.TotalMinutes;
    }
    public async Task RemoveExpiredJwtToken()
    {
        await _localStorageService.RemoveItemAsync("authenticationToken");
    }

    public async Task<string> GetJwtTokenAsync()
    {
        var token = await _localStorageService.GetItemAsync<string>("authenticationToken");
        return token ?? "";
    }

    public async Task SetJwtTokenAsync(string token)
    {
        await _localStorageService.SetItemAsync("authenticationToken", token);
    }
        
    public async Task RemoveJwtTokenAsync()
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
