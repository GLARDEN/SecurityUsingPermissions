using Ardalis.Specification;

using Security.Core.Models.UserManagement;

namespace Security.Core.Models.UserManagement.Specifications;
public class CheckForUsersWithSameEmailSpec : Specification<User>
{
    public CheckForUsersWithSameEmailSpec(Guid userId, string email)
    {
        Query
            .Where(user => !user.Id.Equals(userId) &&
              user.Email.ToLower() == email.ToLower());
    }
}
