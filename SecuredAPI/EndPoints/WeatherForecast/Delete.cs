using Ardalis.ApiEndpoints;

using Microsoft.AspNetCore.Mvc;

using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions;
using Security.Core.Permissions.Enums;
using Security.Core.Services.WeatherForecast;

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
    /// Deletes a specified forecast
    /// </summary>
    [HttpPost(DeleteForecastRequest.Route)]
    [HasPermission(Permission.ForecastDelete)]
    public override async Task<DeleteForecastResponse> HandleAsync([FromBody] DeleteForecastRequest request, CancellationToken cancellationToken = default)
    {
        var result = await _forecastService.DeleteForecast(request);

        return result;
    }

}
