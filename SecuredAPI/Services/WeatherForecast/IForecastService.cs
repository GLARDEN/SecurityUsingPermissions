using Security.Shared.Models;

namespace SecuredAPI.Services;

public interface IForecastService
{

    Task<CreateForecastResponse> CreateForecast(CreateForecastRequest request);
    Task<DeleteForecastResponse> DeleteForecast(DeleteForecastRequest request);
    Task<GetByIdForecastResponse> GetForecastById(GetByIdForecastRequest request);
    Task<ListForecastsResponse> ListForecasts(ListUsersRequest request);
    Task<UpdateForecastResponse> UpdateForecast(UpdateForecastRequest request);
}