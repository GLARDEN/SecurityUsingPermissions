using BlazorClient.Interfaces;

using Security.Core.Models;
using Security.Core.Models.WeatherForecast;

using System.Net;

namespace BlazorClient.Services;

public class WeatherForecastUiService
{
    private readonly IHttpService _httpService;

    public WeatherForecastUiService(IHttpService httpService)
    {
        _httpService = httpService;
        
    }

    public async Task<ApiResponse<CreateForecastResponse>> CreateAsync(WeatherForecastDto forecastDto)
    {
        CreateForecastRequest request = new()
        {
            Summary = forecastDto.Summary,
            Date = forecastDto.Date,
            TemperatureC=forecastDto.TemperatureC
        };

        return await _httpService.HttpPutAsync<CreateForecastResponse>(CreateForecastRequest.Route, request);
    }

    public async Task<ApiResponse<ListForecastsResponse>> ListAsync()
    {        
        return await _httpService.HttpGetAsync<ListForecastsResponse>(ListForecastsRequest.Route);
    }

    public async Task<ApiResponse<UpdateForecastResponse>> UpdateAsync(WeatherForecastDto forecastDto)
    {
        UpdateForecastRequest request = new()
        {
            Forecast = forecastDto
        };

        return await _httpService.HttpPutAsync<UpdateForecastResponse>(UpdateForecastRequest.Route, request);
    }

    public async Task<ApiResponse<DeleteForecastResponse>> DeleteAsync(WeatherForecastDto forecastDto)
    {
        DeleteForecastRequest request = new()
        {
           Forecast = forecastDto
        };

        return await _httpService.HttpPostAsync<DeleteForecastResponse>(DeleteForecastRequest.Route, request);
    }
}
