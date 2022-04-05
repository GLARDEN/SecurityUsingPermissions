using Ardalis.Result;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using Security.Data;
using Security.Shared.Models;
using Security.Shared.Models.UserManagement;

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

    public async Task<Result<CreateForecastResponse>> CreateForecast(CreateForecastRequest request)
    {

        WeatherForecast newWeatherForecast = new(request.Summary, request.Date, request.TemperatureC);

        _appDbContext.WeatherForecasts.Add(newWeatherForecast);        

        var result = await _appDbContext.SaveChangesAsync();

        WeatherForecastDto newForecast = _mapper.Map<WeatherForecastDto>(newWeatherForecast);

        WeatherForecastDto weatherForecastDto = _mapper.Map<WeatherForecastDto>(newForecast);

        return Result<CreateForecastResponse>.Success(new CreateForecastResponse()
        {
            Forecast = newForecast,
            Success = true
        });

  
    }
    public async Task<Result<DeleteForecastResponse>> DeleteForecast(DeleteForecastRequest request)
    {
        var forecast = _appDbContext.WeatherForecasts.Find(request.Id);
        if (forecast != null)
        {
            _appDbContext.WeatherForecasts.Remove(forecast);
        }

        var result = await _appDbContext.SaveChangesAsync();
        return Result<DeleteForecastResponse>.Success(new DeleteForecastResponse()
        {
            Success = result == 0 ? false : true
        });

    }
    public async Task<Result<GetByIdForecastResponse>> GetForecastById(GetByIdForecastRequest request)
    {
        GetByIdForecastResponse response = new();
        response.Success = false;

        var foundForecast = await _appDbContext.WeatherForecasts.FindAsync(request.Id);

        if (foundForecast != null)
        {
            WeatherForecastDto forecast = _mapper.Map<WeatherForecastDto>(foundForecast);
     
            return Result<GetByIdForecastResponse>.Success(new GetByIdForecastResponse()
            {
                Forecast = forecast,
                Success = true
            }); 
        }
        else
        {
            return Result<GetByIdForecastResponse>.NotFound();
        }

    }
    public async Task<Result<ListForecastsResponse>> ListForecasts()
    {
        var forecastList = await _appDbContext.WeatherForecasts.ToListAsync(); 
        var forecasts = _mapper.Map<List<WeatherForecastDto>>(forecastList);
        ListForecastsResponse response = new()
        {
            Forecasts = forecasts,
            Success = true
        };

        return Result<ListForecastsResponse>.Success(new ListForecastsResponse()
        {
            Success = true,
            Forecasts = forecasts
        }); ;

        
    }
    public async Task<Result<UpdateForecastResponse>> UpdateForecast(UpdateForecastRequest request)
    {
        UpdateForecastResponse response = new() { Success=false };
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
                //WeatherForecastDto weatherForecastDto = _mapper.Map<WeatherForecastDto>(forecast);
                return Result<UpdateForecastResponse>.Success(new UpdateForecastResponse()
                {
                  //  Forecast = weatherForecastDto,
                    Success = true
                });
            }
            else
            {
                return Result<UpdateForecastResponse>.Error("Failed to update requested forecast.");
            }
           
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Result<UpdateForecastResponse>.Error(ex.Message);
        }
    }
}
