namespace Security.Shared.Models;

public class UpdateForecastRequest
{
    public const string Route = "api/weatherforecast/update";
    public WeatherForecastDto Forecast { get; set; }
}
