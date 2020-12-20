using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace TestApi.MessageBrokers
{
    public class RabbitObjectPool : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMqOption _options;

        private readonly IConnection _connection;

        public RabbitObjectPool(IOptions<RabbitMqOption> optionsAccs)
        {
            _options = optionsAccs.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VHost,
                Uri = new System.Uri("amqps://ccwnyiqy:bmsZBTtAg7stC6nL3oVoqkAxhMfXHh8e@crow.rmq.cloudamqp.com/ccwnyiqy"),
            };
            //factory.Uri = url.Replace("amqp://", "amqps://");
            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
            {
                return true;
            }
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
