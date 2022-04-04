using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models;

public class EditUserResponse
{   
    public bool Success{get;set;}
    public string ErrorMessage { get; set; }

    public EditUserResponse()
    {

    }
}

