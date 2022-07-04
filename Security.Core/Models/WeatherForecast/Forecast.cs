using Security.SharedKernel.Interfaces;

namespace Security.Core.Models.WeatherForecast;

public class Forecast : IAggregateRoot
{
    public int Id { get; set; }
    public string? Summary { get; set; }
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public Forecast(string? summary, DateTime date, int temperatureC)
    {
        Summary = summary;
        Date = date;
        TemperatureC = temperatureC;

    }
}


