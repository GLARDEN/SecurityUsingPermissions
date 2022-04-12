namespace Security.Core.Models.WeatherForecast;

public class UpdateForecastResponse
{
    public WeatherForecastDto Forecast { get; set; } = null!;
    public bool Success { get; set; }
}

