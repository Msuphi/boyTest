using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Persistence.MongoDB;
using RabbitMQ.Client;
using ServiceBuilders;
using Subscriber.Models.Domain;
using System;

namespace TestApi.MessageBrokers
{
    public static class Extensions
    {
        public static IServiceBuilder AddRabbitMq(this IServiceBuilder builder, IConfiguration configuration)
        {
            var rabbitConfig = configuration.GetSection("rabbitMq");
            builder.Services.Configure<RabbitMqOption>(rabbitConfig);

            builder.Services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            builder.Services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitObjectPool>();

            builder.Services.AddSingleton<IMessageBroker, RabbitMq>();
            builder.Services.AddSingleton<IMessageBroker, RabbitMq>();
            builder.AddMongo().AddMongoRepository<TextMessage, Guid>("TextMessage");

            return builder;
        }
    }
}
