using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;



using Ardalis.Result.AspNetCore;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;

namespace SecuredAPI.EndPoints.WeatherForecasts;


public class List : EndpointBaseAsync
                        .WithoutRequest
                         .WithActionResult<ListForecastsResponse>
{
    private readonly IForecastService _forecastService;

    public List(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    /// <summary>
    /// Get a list of weather forecasts
    /// </summary>
    [HttpGet(ListForecastsRequest.Route)]
    [HasPermission(Permission.ForecastView)]
    public override async Task<ActionResult<ListForecastsResponse>> HandleAsync( CancellationToken cancellationToken = default)
    {
        ListForecastsResponse response = await _forecastService.ListForecasts();

        return this.ToActionResult<ListForecastsResponse>(response);

        
    }
}
