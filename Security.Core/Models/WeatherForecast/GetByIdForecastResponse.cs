namespace Security.Core.Models.WeatherForecast;

public class GetByIdForecastResponse
{
    public WeatherForecastDto Forecast { get; set; } = null!;
    public bool Success { get; set; }


}

