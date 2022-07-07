using Ardalis.Result;

using BlazorClient.Interfaces;

using Microsoft.AspNetCore.Components;

using Security.Core.Models;
using Security.Core.Models.Authentication;

using System.Net;

namespace BlazorClient.Features;

public partial class Register : ComponentBase
{
    [Inject]
    private IAuthenticationUiService UserService { get; set; }
    
    private string PageTitle = "Register";
    private List<string>? _messages = new();
    private RegistrationRequestDto registrationRequest = new();

    private async Task HandleRegistration() 
    {                
        ApiResponse<RegistrationResponse> apiResponse = await UserService.RegisterUserAsync(registrationRequest);
        if (apiResponse.StatusCode != HttpStatusCode.OK)
{
            _messages = apiResponse?.ResponseMessages;
        }
        else
        {
            _messages = null;
        }        
    }
}
