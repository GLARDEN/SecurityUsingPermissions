using Ardalis.Specification;

using Security.Core.Models.Authentication;
using Security.Core.Models.UserManagement;

namespace Security.Core.Models.UserManagement.Specifications;
public class CheckForUniqueRefreshTokenSpec : Specification<User>
{
    public CheckForUniqueRefreshTokenSpec(Guid userId, Guid deviceId, string refreshToken)
    {
        Query
            .Where(u =>u.Id.Equals(userId) && 
                       u.RefreshTokens.Any(t => t.DeviceId.Equals(deviceId) &&
                                                 t.Token.ToLower() == refreshToken.ToLower() &&
                                                 !t.IsInvalid));
    }
}
