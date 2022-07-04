namespace Security.Core.Models.WeatherForecast;

public class DeleteForecastRequest
{
    public const string Route = "api/weatherforecast/delete";
    public WeatherForecastDto Forecast { get; set; }
    public bool Success { get; set; }
}
