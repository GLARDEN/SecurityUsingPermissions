using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.UserManagement;
using Security.Core.Models.UserManagement.Services;
using Security.Core.Models.WeatherForecast;

namespace SecuredAPI.EndPoints.Administration.UserManagement;

public class List : EndpointBaseAsync
                        .WithoutRequest
                        .WithActionResult<ListUsersResponse>
{
    private readonly IUserManagementService _userManagementService;

    public List(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }


    /// <summary>
    /// Get a list of weather forecasts
    /// </summary>
    [HttpGet(ListUsersRequest.Route)]
    
    public override async Task<ActionResult<ListUsersResponse>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var response = await _userManagementService.ListAsync();

        return this.ToActionResult<ListUsersResponse>(response);


    }
}