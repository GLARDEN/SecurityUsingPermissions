using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using Security.Shared.Models;

using Security;
using Security.Shared;
using SecuredAPI.Services;
using Security.Shared.Permissions;
using Ardalis.Result.AspNetCore;

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
