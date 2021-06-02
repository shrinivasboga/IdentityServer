using CodeGenerator.Sample.Server.BlazorWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Server.BlazorWebApp.Services
{
    public interface IWeatherForecastService
    {
        Task<IEnumerable<WeatherForecast>> GetForecastAsync(DateTime startDate);
    }
}
