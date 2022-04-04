using Microsoft.AspNetCore.Components;
using BlazorClient.Services;
using Security.Shared.Models;
using Security.Shared.Permissions.Enums;
using Security.Shared.Permissions;
using System.Security.Claims;
using Security.Shared.Permissions.Helpers;

namespace BlazorClient.Pages
{
    
    public partial class FetchData : ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; }
        [Inject]
        public WeatherForecastService WeatherForecastService { get; set; }
        private List<WeatherForecastDto> _forecasts;
        private ClaimsPrincipal _currentUser;
        private bool _canView = false;
        private bool _canDelete = false;
        private bool _canUpdate = false;
        private bool _canCreate = false;

        public FetchData(){ }

        protected override async Task OnInitializedAsync()
        {
            if (WeatherForecastService != null) 
            {
                _forecasts = await WeatherForecastService.ListAsync();
            }

            if (UserService != null)
            {                
                _currentUser = await UserService.GetCurrentUser();
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
            if(_currentUser.HasPermission(Permission.ForecastView))
            {
                forecast.IsEditing = true;
            }
        }

    
        private async Task Update(WeatherForecastDto forecast)
        {            
            if(forecast.Id > 0)
            {
                var updatedForecast = await WeatherForecastService.UpdateAsync(forecast);
                forecast.Summary = updatedForecast.Summary;
                forecast.IsEditing = updatedForecast.IsEditing;
                forecast.TemperatureC = updatedForecast.TemperatureC;
            }
            else
            {
                var createdForecast = await WeatherForecastService.CreateAsync(forecast);

                forecast.IsEditing = false;

            }
        }

        private async Task Delete(WeatherForecastDto forecast)
        {
            var result = await WeatherForecastService.DeleteAsync(forecast);
            if (result.Success)
            {
                _forecasts = await WeatherForecastService.ListAsync();
            }
        }

        private async Task Add()
        {
            WeatherForecastDto newForecast = new();
            newForecast.IsEditing = true;
            _forecasts.Add(newForecast);

            
        }


    }
}