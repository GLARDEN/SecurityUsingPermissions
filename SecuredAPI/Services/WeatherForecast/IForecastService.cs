using Ardalis.Result;

using Security.Shared.Models;

namespace SecuredAPI.Services;

public interface IForecastService
{

    Task<Result<CreateForecastResponse>> CreateForecast(CreateForecastRequest request);
    Task<Result<DeleteForecastResponse>> DeleteForecast(DeleteForecastRequest request);
    Task<Result<GetByIdForecastResponse>> GetForecastById(GetByIdForecastRequest request);
    Task<Result<ListForecastsResponse>> ListForecasts();
    Task<Result<UpdateForecastResponse>> UpdateForecast(UpdateForecastRequest request);
}