namespace Security.Shared.Models;

public class GetByIdForecastResponse
{
    public WeatherForecastDto Forecast { get; set; } = null!;
    public bool Success{get;set;}
    

}

