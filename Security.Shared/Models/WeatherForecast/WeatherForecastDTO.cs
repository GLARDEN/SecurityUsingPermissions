using System.Text.Json.Serialization;

namespace Security.Shared.Models;

public class WeatherForecastDto
{
    public int Id { get; set; }
    public string Summary { get; set; } = null!;
    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF { get; set; }

    [JsonIgnore]
    public bool IsEditing { get; set; }

}


