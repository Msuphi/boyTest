using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistence.MongoDB;
using Subscriber.Models.Domain;
using TestApi.MessageBrokers;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProducerController : ControllerBase
    {
        private IMessageBroker _broker;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ProducerController> _logger;
        private readonly IMongoRepository<TextMessage, Guid> _mongoRepository;
        public ProducerController(ILogger<ProducerController> logger, IMessageBroker broker, IMongoRepository<TextMessage, Guid> mongoRepository)
        {
            _logger = logger;
            _broker = broker;
            _mongoRepository = mongoRepository;
        }

        [HttpPost]
        [Route("Messages/{mesaj}")]
        public void Post([FromBody]TextMessage mesaj)
        { 
            _broker.Publish(mesaj, "test.textmessage.netcore", "topic", "*.queue.TextQueue.dotnetcore.#");

            
        }
        [HttpGet]
        [Route("Messages")]
        public IEnumerable<string> Get()
        {
           var messages =  _mongoRepository.FindAsync(x => x.Timestamp < DateTime.Now).GetAwaiter().GetResult().OrderBy(x => x.Timestamp);

            return messages.Select(x=>x.Message);
        }
    }
}
