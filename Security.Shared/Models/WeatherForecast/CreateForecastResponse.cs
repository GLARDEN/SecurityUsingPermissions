namespace Security.Shared.Models;

public class CreateForecastResponse
{
    public WeatherForecastDto Forecast { get; set; } = null!;
    public bool Success{get;set;}
    

}

