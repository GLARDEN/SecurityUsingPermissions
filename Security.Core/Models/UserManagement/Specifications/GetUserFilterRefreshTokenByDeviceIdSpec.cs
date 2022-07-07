using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUserFilterRefreshTokenByDeviceIdSpec : Specification<User>, ISingleResultSpecification
{
    public GetUserFilterRefreshTokenByDeviceIdSpec(Guid userId,Guid deviceId)
    {
        Query
            .Where(user => user.Id.Equals(userId))
            .Include(u => u.UserRoles)
            .Include(u => u.RefreshTokens.Where(rt => rt.DeviceId.Equals(deviceId)))
            .IgnoreQueryFilters();
    }
}