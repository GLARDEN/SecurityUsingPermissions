namespace Security.Shared.Models;

public class WeatherForecast
{
    public int Id { get; set; }
    public string? Summary { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public WeatherForecast(string? summary, DateTime date, int temperatureC)
    {
        Summary=summary;
        Date=date;
        TemperatureC=temperatureC;

    }
}


