using Persistence.MongoDB.Factories;
using Persistence.MongoDB.Initializers;
using Persistence.MongoDB.Seeders;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Persistence.MongoDB.Builders;
using Persistence.MongoDB.Repositories;
using ServiceBuilders;

namespace Persistence.MongoDB
{
    public static class Extensions
    {
        private static bool _conventionsRegistered;
        private const string SectionName = "mongo";

        public static IServiceBuilder AddMongo(this IServiceBuilder builder, string sectionName = SectionName,
            Type seederType = null, bool registerConventions = true)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }

            var mongoOptions = builder.GetOptions<MongoDbOptions>(sectionName);
            return builder.AddMongo(mongoOptions, seederType, registerConventions);
        }

        public static IServiceBuilder AddMongo(this IServiceBuilder builder, Func<IMongoDbOptionsBuilder,
            IMongoDbOptionsBuilder> buildOptions, Type seederType = null, bool registerConventions = true)
        {
            var mongoOptions = buildOptions(new MongoDbOptionsBuilder()).Build();
            return builder.AddMongo(mongoOptions, seederType, registerConventions);
        }

        public static IServiceBuilder AddMongo(this IServiceBuilder builder, MongoDbOptions mongoOptions,
            Type seederType = null, bool registerConventions = true)
        {
            builder.Services.AddSingleton(mongoOptions);
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                return new MongoClient(options.ConnectionString);
            });
            builder.Services.AddTransient(sp =>
            {
                var options = sp.GetService<MongoDbOptions>();
                var client = sp.GetService<IMongoClient>();
                return client.GetDatabase(options.Database);
            });
            builder.Services.AddTransient<IMongoDbInitializer, MongoDbInitializer>();
            builder.Services.AddTransient<IMongoSessionFactory, MongoSessionFactory>();

            if (seederType is null)
            {
                builder.Services.AddTransient<IMongoDbSeeder, MongoDbSeeder>();
            }
            else
            {
                builder.Services.AddTransient(typeof(IMongoDbSeeder), seederType);
            }

            builder.AddInitializer<IMongoDbInitializer>();
            
            return builder;
        }
        public static IServiceBuilder AddMongoRepository<TEntity, TIdentifiable>(this IServiceBuilder builder,
          string collectionName)
          where TEntity : IIdentifiable<TIdentifiable>
        {
            builder.Services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
            {
                var database = sp.GetService<IMongoDatabase>();
                return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
            });

            return builder;
        }
    }
}