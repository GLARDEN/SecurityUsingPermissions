using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Security.Core.Permissions;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions.Enums;

namespace SecuredAPI.EndPoints.WeatherForecasts;


public class Create : EndpointBaseAsync
                        .WithRequest<CreateForecastRequest>
                        .WithResult<CreateForecastResponse>
{
    private readonly IForecastService _forecastService;

    public Create(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    /// <summary>
    /// Get a list of weather forecasts
    /// </summary>
    [HttpPut(CreateForecastRequest.Route)]
    [HasPermission(Permission.ForecastCreate)]
    public override async Task<CreateForecastResponse> HandleAsync([FromBody] CreateForecastRequest request, CancellationToken cancellationToken = default)
{
        return await _forecastService.CreateForecast(request);
       
    }


    //private async Task<WeatherForecastResponse> GetWeatherForecastResultAsync()
    //{
    //    var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    //    var weatherForecasts = new List<WeatherForecast>();

    //    for (int index = 0; index <=4; index++)
    //    {
    //        var weatherForecast = new WeatherForecast(
    //           summaries[Random.Shared.Next(summaries.Length)],
    //           DateTime.Now.AddDays(index),
    //           Random.Shared.Next(-20, 55)
    //        );
    //        weatherForecasts.Add(weatherForecast);
    //    }


    //    var result = await Task.Run(() => weatherForecasts.Select(i => _mapper.Map<WeatherForecastDTO>(i)).ToList());

    //    var response = new WeatherForecastResponse()
    //    {
    //        Count = result.Count,
    //        WeatherForecasts=result
    //    };

    //    return response;

    // }
}
