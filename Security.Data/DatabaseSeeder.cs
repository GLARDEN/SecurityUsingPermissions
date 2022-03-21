using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

using SecuredAPI.Services;

using Security.Shared.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security.Data;
public class DatabaseCreator : IDatabaseCreator
{
    private readonly AppDbContext _appDbContext;

    public DatabaseCreator(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void Initialize()
    {

        CreateForecasts();
    }

    private void CreateForecasts()
    {
        Task.Run(async () =>
        {
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var weatherForecasts = new List<WeatherForecast>();

            for (int index = 0; index <= summaries.Length; index++)
            {
                var weatherForecast = new WeatherForecast(
                   summaries[Random.Shared.Next(summaries.Length)],
                   DateTime.Now.AddDays(index),
                   Random.Shared.Next(-20, 55)
                );
                weatherForecasts.Add(weatherForecast);
            }
            await _appDbContext.WeatherForecasts.AddRangeAsync(weatherForecasts);
            await _appDbContext.SaveChangesAsync();

        }).GetAwaiter().GetResult();
    }
}

