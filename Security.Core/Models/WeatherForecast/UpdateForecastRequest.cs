namespace Security.Core.Models.WeatherForecast;

public class UpdateForecastRequest
{
    public const string Route = "api/weatherforecast/update";
    public WeatherForecastDto Forecast { get; set; }
}
