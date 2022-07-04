using Ardalis.Result;

using Security.Core.Models;
using Security.Core.Models.WeatherForecast;

namespace Security.Core.Services.WeatherForecast;

public interface IForecastService
{

    Task<Result<CreateForecastResponse>> CreateForecast(CreateForecastRequest request);
    Task<Result<DeleteForecastResponse>> DeleteForecast(DeleteForecastRequest request);
    Task<Result<GetByIdForecastResponse>> GetForecastById(GetByIdForecastRequest request);
    Task<Result<ListForecastsResponse>> ListForecasts();
    Task<Result<UpdateForecastResponse>> UpdateForecast(UpdateForecastRequest request);
}