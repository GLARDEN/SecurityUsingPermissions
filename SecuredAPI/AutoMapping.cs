using AutoMapper;

using Security.Shared.Models;

namespace SecuredAPI;

public class AutoMapping :Profile
{
    public AutoMapping()
    {
        CreateMap<WeatherForecast, WeatherForecastDto>();
    }
}
