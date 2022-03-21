namespace Security.Shared.Models;

public class ListForecastsResponse
{
    public IEnumerable<WeatherForecastDto> Forecasts { get; set; } = null!;
    public bool Success{get;set;}
    

}

