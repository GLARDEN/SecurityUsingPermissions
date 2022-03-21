using BlazorClient.Services;

using Microsoft.AspNetCore.Components;

using Security.Shared.Models;
using Security.Shared.Models.Authentication;

namespace BlazorClient.Pages;

public partial class Register {
    [Inject]
    private IUserService AuthenticationService { get; set; }

    private RegistrationRequestDto registrationRequest = new();
    private string PageTitle = "Register";
    private string ErrorMessage = string.Empty;
    private async Task HandleRegistration() {
                
        RegistrationResponseDto result = await AuthenticationService.RegisterUserAsync(registrationRequest);
        if (result.IsRegistrationSuccessful)
        {
            ErrorMessage = result.Errors.FirstOrDefault() ?? "";
        }
        else
        {
            ErrorMessage = string.Empty;
        }

        
    }
}
