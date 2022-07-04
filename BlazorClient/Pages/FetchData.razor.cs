using Microsoft.AspNetCore.Components;
using BlazorClient.Services;
using System.Security.Claims;
using Security.Core.Models.WeatherForecast;
using Security.Core.Permissions.Helpers;
using Security.Core.Permissions.Enums;
using Security.Core.Models;
using System.Net;
using BlazorClient.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorClient.Providers;

namespace BlazorClient.Pages
{

    public partial class FetchData : IDisposable
    {
        [CascadingParameter]
        public AuthStateProvider authenticationStateProvider { get; set; }

       // [Inject]
      //  public IHttpInterceptorService Interceptor { get; set; }

        [Inject]
        public IAuthenticationUiService UserService { get; set; }

        [Inject]
        public WeatherForecastUiService WeatherForecastService { get; set; }
        
        private List<WeatherForecastDto>? _forecasts;
        private ClaimsPrincipal _currentUser;
        private bool _canView = false;
        private bool _canDelete = false;
        private bool _canUpdate = false;
        private bool _canCreate = false;

        public FetchData() { }

        protected override void OnInitialized()
        {
       //     Interceptor.RegisterEvent();
        }

        protected override async Task OnInitializedAsync()
        {
            if (WeatherForecastService != null)
            {
                ApiResponse<ListForecastsResponse> apiResponse = await WeatherForecastService.ListAsync();
                if (apiResponse.StatusCode == HttpStatusCode.OK)
                {
                    _forecasts = apiResponse.Data?.Forecasts.ToList();
                }
            }

            if (UserService != null)
            {
                _currentUser = (await authenticationStateProvider.GetAuthenticationStateAsync()).User;                 
                _canCreate = _currentUser.HasPermission(Permission.ForecastCreate);
                _canUpdate = _currentUser.HasPermission(Permission.ForecastUpdate);
                _canDelete = _currentUser.HasPermission(Permission.ForecastDelete);
            }

            await base.OnInitializedAsync();
        }

        private string GetRowClass(bool shouldHideRow)
        {
            return shouldHideRow ? "row visible" : "row invisible";
        }

        private void Edit(WeatherForecastDto forecast)
        {
            if (_currentUser.HasPermission(Permission.ForecastView))
            {
                forecast.IsEditing = true;
            }
        }

        private async Task Update(WeatherForecastDto forecast)
        {
            if (forecast.Id > 0)
            {
                await WeatherForecastService.UpdateAsync(forecast);
                //forecast.Summary = updatedForecast.Summary;
                //forecast.IsEditing = updatedForecast.IsEditing;
                //forecast.TemperatureC = updatedForecast.TemperatureC;
            }
            else
            {
                var createdForecast = await WeatherForecastService.CreateAsync(forecast);

                forecast.IsEditing = false;

            }
        }

        private async Task Delete(WeatherForecastDto forecast)
        {
            ApiResponse<DeleteForecastResponse> apiDeleteResponse = await WeatherForecastService.DeleteAsync(forecast);
            if (apiDeleteResponse.StatusCode == HttpStatusCode.OK)
            {
                ApiResponse<ListForecastsResponse> apiListResponse = await WeatherForecastService.ListAsync();
                if (apiListResponse.StatusCode == HttpStatusCode.OK)
                {
                    _forecasts = apiListResponse.Data?.Forecasts.ToList();
                }
            }
            else
            {
                //Display error messages or validation messages
            }
        }

        private async Task Add()
        {
            WeatherForecastDto newForecast = new();
            newForecast.IsEditing = true;
            _forecasts?.Add(newForecast);
        }

        public void Dispose()
        {
          
          //  Interceptor.DisposeEvent();
        }
    }
}