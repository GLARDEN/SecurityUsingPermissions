namespace Security.Shared.Models;

public class CreateForecastRequest
{
    public const string Route = "api/weatherforecast/create";
    public string? Summary { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
}
