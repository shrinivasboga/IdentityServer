using CodeGenerator.Sample.Client.BlazorWebApp.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.BlazorWebApp.Pages
{
    public partial class FetchData
    {
        private WeatherForecast[] forecasts;

        protected override async Task OnInitializedAsync()
        {
            forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("/WeatherForecast");
        }
    }
}
