using Ardalis.Result;

using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement.Specifications;
using Security.Core.Models.UserManagement;
using Security.Core.Services;
using Security.SharedKernel.Interfaces;

namespace SecuredAPI.Services;

public class RefreshTokenService : IRefreshTokenService
{

    private readonly IHashService _hashService;
    private readonly IRepository<User> _userRepository;

    public RefreshTokenService(IHashService hashService, IRepository<User> userRepository)
    {
        _hashService = hashService;
        _userRepository = userRepository;
    }

    public bool ValidateRefreshToken(string? token, byte[] tokenHash, byte[] tokenSalt)
    {
        if(token == null)
        {
            return false;
        }

        return _hashService.VerifyHash(token, tokenHash, tokenSalt);
    }

    public RefreshToken GenerateRefreshToken(UserTokenDetails userTokenDetails)
    {
        string newTokenValue = RefreshToken.GenerateTokenValue();

        _hashService.CreateHash(newTokenValue, out byte[] tokenHash, out byte[] tokenSalt);

        RefreshToken newRefreshToken = new(userTokenDetails.Id, userTokenDetails.DeviceId, 
                                            DateTime.Now, DateTime.Now.AddDays(7), newTokenValue, tokenHash, tokenSalt);

        return newRefreshToken;
    }


    public async Task<Result<RevokeRefreshTokenResponse>> RevokeUserRefreshTokenAsync(RevokeRefreshTokenRequest revokeRefreshTokenRequest)
    {
        RevokeRefreshTokenResponse refreshTokenResponse = new();

        User? user = await _userRepository.GetBySpecAsync(new GetUserFilterRefreshTokenByDeviceIdSpec(revokeRefreshTokenRequest.UserId, revokeRefreshTokenRequest.DeviceId));

        if (user != null && user.RefreshTokens.Any())
        {

            user.RevokeRefreshToken(revokeRefreshTokenRequest.DeviceId);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var revokeRefreshTokenResponse = new RevokeRefreshTokenResponse()
            {
                UserId = user.Id
            };

            return Result<RevokeRefreshTokenResponse>.Success(revokeRefreshTokenResponse);
        }

        return Result<RevokeRefreshTokenResponse>.Error();
    }

    public async Task<Result<RevokeRefreshTokenResponse>> RevokeUserRefreshTokensAsync(RevokeRefreshTokenRequest revokeRefreshTokenRequest)
    {
        RevokeRefreshTokenResponse refreshTokenResponse = new();

        User? user = await _userRepository.GetBySpecAsync(new GetUserIncludeRefreshTokensSpec(revokeRefreshTokenRequest.UserId));

        if (user != null && user.RefreshTokens.Any())
        {

            user.RevokeAllRefreshTokens();
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var revokeRefreshTokenResponse = new RevokeRefreshTokenResponse()
            {
                UserId = user.Id
            };

            return Result<RevokeRefreshTokenResponse>.Success(revokeRefreshTokenResponse);
        }

        return Result<RevokeRefreshTokenResponse>.Error();
    }

}
