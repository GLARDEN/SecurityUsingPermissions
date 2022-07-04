using Ardalis.Specification;

namespace Security.Core.Models.WeatherForecast.Specifications;
public class GetWeatherForecastByIdSpec : Specification<Forecast>, ISingleResultSpecification
{
    public GetWeatherForecastByIdSpec(int forecastId)
    {
        Query
            .Where(forecast => forecast.Id.Equals(forecastId));
    }
}

