namespace Security.Shared.Models;

public class CreateForecastRequest
{
    public string? Summary { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
}
