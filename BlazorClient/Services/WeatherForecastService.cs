using Security.Core.Models;
using Security.Core.Models.WeatherForecast;

namespace BlazorClient.Services;

public class WeatherForecastService
{
    private readonly IHttpService _httpService;

    public WeatherForecastService(IHttpService httpService)
    {
        _httpService = httpService;
        
    }

    public async Task<WeatherForecastDto> CreateAsync(WeatherForecastDto forecastDto)
    {
        CreateForecastRequest request = new()
        {
            Summary = forecastDto.Summary,
            Date = forecastDto.Date,
            TemperatureC=forecastDto.TemperatureC
        };

        var response = await _httpService.HttpPutAsync<CreateForecastResponse>(CreateForecastRequest.Route, request);
        if (response.Success)
        {
            forecastDto = response.Forecast;
            forecastDto.IsEditing = false;

        }

        return forecastDto;
    }

    public async Task<List<WeatherForecastDto>> ListAsync()
    {        
        var response = await _httpService.HttpGetAsync<ListForecastsResponse>(ListForecastsRequest.Route);
        if (response.Forecasts != null) 
        {
            return response.Forecasts.ToList();
        }
        else
        {
            return null;
        }
    }

    public async Task UpdateAsync(WeatherForecastDto forecastDto)
    {
        UpdateForecastRequest request = new()
        {
            Forecast = forecastDto
        };

        var response = await _httpService.HttpPutAsync<UpdateForecastResponse>(UpdateForecastRequest.Route, request);
        if (response.Success)
        { 
            forecastDto.IsEditing = false;
        }
    }

    public async Task<DeleteForecastResponse> DeleteAsync(WeatherForecastDto forecastDto)
    {
        DeleteForecastRequest request = new()
        {
            Id = forecastDto.Id
        };

        var response = await _httpService.HttpPostAsync<DeleteForecastResponse>(DeleteForecastRequest.Route, request);
        return response;
    }
}
