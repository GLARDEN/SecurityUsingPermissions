namespace Security.Core.Models.WeatherForecast;

public class CreateForecastResponse
{
    public WeatherForecastDto Forecast { get; set; } = null!;
    public bool Success { get; set; }


}

