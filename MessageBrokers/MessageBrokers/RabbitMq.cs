using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using Persistence.MongoDB;
using PushNotifications;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Subscriber.Models.Domain;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestApi.MessageBrokers
{
    public class RabbitMq : IMessageBroker
    {
        private readonly DefaultObjectPool<IModel> _objectPool;
        private readonly IMongoRepository<TextMessage, Guid> _mongoRepository;
        public RabbitMq(IPooledObjectPolicy<IModel> objectPolicy, IMongoRepository<TextMessage, Guid> mongoRepository)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
            _mongoRepository = mongoRepository;
        }
        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey)
       where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();

            try
            {
                channel.ExchangeDeclare(exchangeName, exchangeType, true, false, null);

                var sendBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchangeName, routeKey, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        public Task Subscribe<T>(CancellationToken stoppingToken, string queueName) where T : class
        {
            var channel = _objectPool.Get();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var seriBody = Encoding.UTF8.GetString(body);
                var message =  JsonConvert.DeserializeObject<TextMessage>(seriBody);
                try
                {
                    _mongoRepository.AddAsync(
                            new TextMessage(Guid.NewGuid(), message.Message, DateTime.Now,message.DeviceToken)
                        );

                    FirebaseNotification.SendNotification(message.Message, message.DeviceToken).GetAwaiter();
                }
                catch (Exception)
                {

                    throw;
                }
                Console.WriteLine(" [x] Received {0}", message);

            };


            channel.BasicConsume(queueName, false, consumer);
            return Task.CompletedTask;

        }

    }
}
