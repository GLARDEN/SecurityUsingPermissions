using Security.Shared.Models;

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


        var response = await _httpService.HttpPutAsync<CreateForecastResponse>("weatherforecasts/create", request);
        if (response.Success)
        {
            forecastDto = response.Forecast;
            forecastDto.IsEditing = false;

        }

        return forecastDto;
    }

    public async Task<List<WeatherForecastDto>> ListAsync()
    {        
        var response = await _httpService.HttpPostAsync<ListForecastsResponse>("weatherforecasts/list", new ListUsersRequest());
        if (response.Forecasts != null) 
        {
            return response.Forecasts.ToList();
        }
        else
        {
            return null;
        }
    }

    public async Task<WeatherForecastDto> UpdateAsync(WeatherForecastDto forecastDto)
    {
        UpdateForecastRequest request = new()
        {
            Forecast = forecastDto
        };


        var response = await _httpService.HttpPutAsync<UpdateForecastResponse>("weatherforecasts/update", request);
        if (response.Success)
        {
            forecastDto = response.Forecast;
            forecastDto.IsEditing = false;
           
        }

        return forecastDto;
    }

    public async Task<DeleteForecastResponse> DeleteAsync(WeatherForecastDto forecastDto)
    {
        DeleteForecastRequest request = new()
        {
            Id = forecastDto.Id
        };

        var response = await _httpService.HttpPostAsync<DeleteForecastResponse>("weatherforecasts/delete", request);
        return response;
    }
}
