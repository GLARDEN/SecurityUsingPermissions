using Ardalis.Specification;

namespace Security.Core.Models.UserManagement.Specifications;

public class GetUserAndAssignedRolesByIdSpec : Specification<User>, ISingleResultSpecification
{
    public GetUserAndAssignedRolesByIdSpec(Guid userId)
    {
        Query.Include(user => user.UserRoles).Where(user => user.Id.Equals(userId));
    }
}
