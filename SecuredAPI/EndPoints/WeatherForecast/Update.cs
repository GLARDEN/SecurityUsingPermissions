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


public class Update : EndpointBaseAsync
                        .WithRequest<UpdateForecastRequest>
                        .WithResult<UpdateForecastResponse>
{
    private readonly IForecastService _forecastService;

    public Update(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    /// <summary>
    /// Get a list of weather forecasts
    /// </summary>
    [HttpPut("api/weatherforecasts/update")]
    [HasPermission(Permission.ForecastUpdate)]
    public override async Task<UpdateForecastResponse> HandleAsync([FromBody] UpdateForecastRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _forecastService.UpdateForecast(request);

        return result;
    }
}
