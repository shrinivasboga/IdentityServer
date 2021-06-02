using CodeGenerator.Sample.Client.MvcWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.MvcWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public IActionResult Login()
        {
            ViewBag.Message = "You are now Logged in!!";
            return View("Index");
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public async Task<IActionResult> CallWeatherApi()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = await client.GetStringAsync("https://localhost:44322/WeatherForecast");

                ViewBag.Json = JArray.Parse(content).ToString();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Json = $"API Request Failed.\nHTTP Status Code : {ex.StatusCode}\nHTTP Status Message : {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Json = $"Something went wrong.\nException Details : \n{ex.Message}";
            }
            return View("json");
        }

        public async Task<IActionResult> CallAccessTokenApi()
        {
            try
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");

                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var content = await client.GetStringAsync("https://localhost:44322/identity");

                ViewBag.Json = JArray.Parse(content).ToString();
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Json = $"API Request Failed.\nHTTP Status Code : {ex.StatusCode}\nHTTP Status Message : {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.Json = $"Something went wrong.\nException Details : \n{ex.Message}";
            }
            return View("json");
        }
    }
}
