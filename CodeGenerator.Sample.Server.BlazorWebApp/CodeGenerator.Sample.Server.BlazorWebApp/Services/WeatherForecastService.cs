using CodeGenerator.Sample.Server.BlazorWebApp.Models;
using CodeGenerator.Sample.Server.BlazorWebApp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Server.BlazorWebApp.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly HttpClient httpClient;

        public WeatherForecastService(HttpClient httpClient,
                                      TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider.AccessToken);
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync(DateTime startDate)
        {
            var response = await httpClient.GetAsync("/weatherforecast");
            if (!response.IsSuccessStatusCode)
            {
                return new List<WeatherForecast>();
            }
            else
            {
                return new List<WeatherForecast>();
            }
        }
    }
}
