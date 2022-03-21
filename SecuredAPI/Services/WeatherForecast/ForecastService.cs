using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Security.Data;
using Security.Shared.Models;

namespace SecuredAPI.Services;

public class ForecastService : IForecastService
{
    private readonly IMapper _mapper;
    private readonly AppDbContext _appDbContext;

    public ForecastService(IMapper mapper, AppDbContext appDbContext)
    {
        _mapper = mapper;
        _appDbContext = appDbContext;
    }

    public async Task<CreateForecastResponse> CreateForecast(CreateForecastRequest request)
    {

        WeatherForecast newWeatherForecast = new(request.Summary, request.Date, request.TemperatureC);

        _appDbContext.WeatherForecasts.Add(newWeatherForecast);        

        var result = await _appDbContext.SaveChangesAsync();

        WeatherForecastDto newForecast = _mapper.Map<WeatherForecastDto>(newWeatherForecast);

        CreateForecastResponse forecastResponse = new(){Forecast = newForecast,Success = true };

        return forecastResponse;
    }
    public async Task<DeleteForecastResponse> DeleteForecast(DeleteForecastRequest request)
    {
        var forecast = _appDbContext.WeatherForecasts.Find(request.Id);
        if (forecast != null)
        {
            _appDbContext.WeatherForecasts.Remove(forecast);
        }

        var result = await _appDbContext.SaveChangesAsync();

        DeleteForecastResponse response = new()
        {
            Success = result == 0 ? false : true
        };

        return response;
    }
    public async Task<GetByIdForecastResponse> GetForecastById(GetByIdForecastRequest request)
    {
        GetByIdForecastResponse response = new();
        response.Success = false;

        var foundForecast = await _appDbContext.WeatherForecasts.FindAsync(request.Id);

        if (foundForecast != null)
        {
            WeatherForecastDto forecast = _mapper.Map<WeatherForecastDto>(foundForecast);
            response.Forecast = forecast;
            response.Success = true;
        }

        return response;

    }
    public async Task<ListForecastsResponse> ListForecasts(ListForecastsRequest request)
    {
        var forecastList = await _appDbContext.WeatherForecasts.ToListAsync(); 
        var forecasts = _mapper.Map<List<WeatherForecastDto>>(forecastList);
        ListForecastsResponse response = new()
        {
            Forecasts = forecasts,
            Success = true
        };

        return response;
    }
    public async Task<UpdateForecastResponse> UpdateForecast(UpdateForecastRequest request)
    {
        UpdateForecastResponse response = new() {Forecast = null, Success=false };
        try
        {
            WeatherForecast? forecast = await _appDbContext.WeatherForecasts.FindAsync(request.Forecast.Id);

            if (forecast != null)
            {
                forecast.Summary = request.Forecast.Summary;
                forecast.TemperatureC = request.Forecast.TemperatureC;
                forecast.Date = request.Forecast.Date;
            }

            var result = await _appDbContext.SaveChangesAsync();

            if (result != 0)
            {
                WeatherForecastDto weatherForecastDto = _mapper.Map<WeatherForecastDto>(forecast);
                response = new()
                {
                    Forecast = weatherForecastDto,
                    Success = true
                };

            }

            return response;
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return new UpdateForecastResponse() { Success = false };
        }
    }
}
