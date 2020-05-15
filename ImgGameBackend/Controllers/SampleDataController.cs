using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Imaginarium.Models;
using Microsoft.AspNetCore.Mvc;

namespace Imaginarium.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly ImaginariumContext _dbContext;

        public SampleDataController(
            ImaginariumContext dbContext)
        {
            _dbContext = dbContext;
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts(int startDateIndex)
        {
            var xy = _dbContext.Users.Any();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index + startDateIndex).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }


        [HttpGet("createGame")]
        public async Task<IActionResult> CreateGame()
        {
            var xy = _dbContext.Users.Any();
            var x = 4567;
            return Ok(x);
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }
    }
}
