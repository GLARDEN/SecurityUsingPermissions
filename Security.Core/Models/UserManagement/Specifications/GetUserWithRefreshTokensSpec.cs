using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUserWithRefreshTokensSpec : Specification<User>, ISingleResultSpecification
{
    public GetUserWithRefreshTokensSpec(Guid userId)
    {
        Query.Where(user => user.Id.Equals(userId))
             .Include(u => u.UserRoles)
             .Include(u => u.RefreshTokens);
    }
}
