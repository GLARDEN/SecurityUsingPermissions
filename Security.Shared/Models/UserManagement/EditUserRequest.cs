using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models;

public class EditUserRequest
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

   
}

