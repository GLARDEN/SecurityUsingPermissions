using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using Security.Shared.Models;

using Security;
using Security.Shared;
using SecuredAPI.Services;
using Security.Shared.Permissions;

namespace SecuredAPI.EndPoints.WeatherForecasts;


public class List : EndpointBaseAsync
                        .WithRequest<ListUsersRequest>
                        .WithResult<ListForecastsResponse>
{
    private readonly IForecastService _forecastService;

    public List(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    /// <summary>
    /// Get a list of weather forecasts
    /// </summary>
    [HttpPost("api/weatherforecasts/list")]
    [HasPermission(Permission.ForecastView)]
    public override async Task<ListForecastsResponse> HandleAsync([FromBody] ListUsersRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _forecastService.ListForecasts(request);

        return result;
    }
}
