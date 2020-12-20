using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence.MongoDB;
using Subscriber.Models.Domain;
using TestApi.MessageBrokers;

namespace Subscriber.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {
        private IMessageBroker _broker;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SubscriberController> _logger;

        public SubscriberController(ILogger<SubscriberController> logger, IMessageBroker broker)
        {
            _logger = logger;
            _broker = broker;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            
            _broker.Subscribe<string>(new System.Threading.CancellationToken(), "TextQueue");
            
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
