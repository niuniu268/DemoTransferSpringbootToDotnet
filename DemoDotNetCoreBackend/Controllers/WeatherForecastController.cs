// using Microsoft.AspNetCore.Mvc;
//
// namespace DemoDotNetCoreBackend.Controllers;
//
// [ApiController]
// [Route("[controller]")]
// public class WeatherForecastController : ControllerBase
// {
//     private static readonly string[] Summaries = new[]
//     {
//         "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//     };
//
//     private readonly ILogger<WeatherForecastController> _logger;
//
//     public WeatherForecastController(ILogger<WeatherForecastController> logger)
//     {
//         _logger = logger;
//         _logger.LogDebug(1, "NLog injected into HomeController");
//     }
//
//     [HttpGet(Name = "GetWeatherForecast")]
//     [TypeFilter(typeof(LogActionAttribute))]
//     public IEnumerable<WeatherForecast> Get()
//     {
//         _logger.LogInformation("Hello, this is the index!");
//         return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//             {
//                 Date = DateTime.Now.AddDays(index),
//                 TemperatureC = Random.Shared.Next(-20, 55),
//                 Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//             })
//             .ToArray();
//     }
// }