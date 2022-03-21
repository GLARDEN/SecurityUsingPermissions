using Ardalis.ApiEndpoints;

using AutoMapper;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


using Security.Shared.Models;

using Security;
using Security.Shared;
using SecuredAPI.Services;

namespace SecuredAPI.EndPoints.WeatherForecasts;


public class Delete : EndpointBaseAsync
                        .WithRequest<DeleteForecastRequest>
                        .WithResult<DeleteForecastResponse>
{
    private readonly IForecastService _forecastService;

    public Delete(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    /// <summary>
    /// Get a list of weather forecasts
    /// </summary>
    [HttpPost("api/weatherforecasts/delete")]
    
    public override async Task<DeleteForecastResponse> HandleAsync([FromBody] DeleteForecastRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _forecastService.DeleteForecast(request);

        return result;
    }

}
