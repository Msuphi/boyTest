using System.Threading;
using System.Threading.Tasks;

namespace TestApi.MessageBrokers
{
    public interface IMessageBroker
    {
        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
        public Task Subscribe<T>(CancellationToken stoppingToken, string queueName) where T : class;
    }
}
