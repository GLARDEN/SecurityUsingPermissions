using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Permissions.Enums;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions;
using Security.Core.Services.WeatherForecast;

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
    [HttpPut(UpdateForecastRequest.Route)]
    [HasPermission(Permission.ForecastUpdate)]
    public override async Task<UpdateForecastResponse> HandleAsync([FromBody] UpdateForecastRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _forecastService.UpdateForecast(request);

        return result;
    }
}
