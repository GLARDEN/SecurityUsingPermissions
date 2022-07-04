using Ardalis.Result;
using AutoMapper;

using Security.Core.Models.WeatherForecast;
using Security.Core.Models.WeatherForecast.Specifications;
using Security.SharedKernel.Interfaces;

namespace Security.Core.Services.WeatherForecast;

public class ForecastService : IForecastService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Forecast> _repository;

    public ForecastService(IMapper mapper,IRepository<Forecast> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<CreateForecastResponse>> CreateForecast(CreateForecastRequest request)
    {

        Forecast newWeatherForecast = new(request.Summary, request.Date, request.TemperatureC);
                
        await _repository.AddAsync(newWeatherForecast);

        var result = await _repository.SaveChangesAsync();

        WeatherForecastDto newForecast = _mapper.Map<WeatherForecastDto>(newWeatherForecast);

        return Result<CreateForecastResponse>.Success(new CreateForecastResponse()
        {
            Id = newForecast.Id           
        });


    }
    public async Task<Result<DeleteForecastResponse>> DeleteForecast(DeleteForecastRequest request)
    {
        Forecast forecast = _mapper.Map<Forecast>(request.Forecast);

        await _repository.DeleteAsync(forecast);

        var result = await _repository.SaveChangesAsync();
        return Result<DeleteForecastResponse>.Success(new DeleteForecastResponse());

    }
    public async Task<Result<GetByIdForecastResponse>> GetForecastById(GetByIdForecastRequest request)
    {
        GetByIdForecastResponse response = new();
        response.Success = false;
        
        var foundForecast = await _repository.GetByIdAsync(new GetWeatherForecastByIdSpec(request.Id));
        if (foundForecast != null)
        {
            WeatherForecastDto forecast = _mapper.Map<WeatherForecastDto>(foundForecast);

            return Result<GetByIdForecastResponse>.Success(new GetByIdForecastResponse()
            {
                Forecast = forecast
            });
        }
        else
        {
            return Result<GetByIdForecastResponse>.NotFound();
        }

    }
    public async Task<Result<ListForecastsResponse>> ListForecasts()
    {
        var forecastList = await _repository.ListAsync();
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
        UpdateForecastResponse response = new();
        try
        {
            Forecast? forecast = await _repository.GetByIdAsync(new GetWeatherForecastByIdSpec(request.Forecast.Id));

            if (forecast != null)
            {
                forecast.Summary = request.Forecast.Summary;
                forecast.TemperatureC = request.Forecast.TemperatureC;
                forecast.Date = request.Forecast.Date;
            }

            var result = await _repository.SaveChangesAsync();

            if (result != 0)
            {   
                return Result<UpdateForecastResponse>.Success(new UpdateForecastResponse());
            }
            else
            {
                return Result<UpdateForecastResponse>.Error("Failed to update requested forecast.");
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Result<UpdateForecastResponse>.Error(ex.Message);
        }
    }
}
