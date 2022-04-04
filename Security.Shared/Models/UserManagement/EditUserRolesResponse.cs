using Security.Shared.Models.UserManagement;

namespace Security.Shared.Models;

public class EditUserRolesResponse
{   
    public bool Success{get;set;}
    public string ErrorMessage { get; set; }

    public EditUserRolesResponse()
    {

    }
}

