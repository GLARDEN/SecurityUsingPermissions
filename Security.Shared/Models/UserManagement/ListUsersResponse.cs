using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models;

public class ListUsersResponse
{
    public List<UserSummaryDto> RegisteredUsers { get; set; }

    public bool Success { get; set; }

    public ListUsersResponse()
    {

    }
}
