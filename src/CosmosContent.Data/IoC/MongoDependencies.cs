using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Security.Authentication;

namespace CosmosContent.Data.IoC
{
    public static class MongoDependencies
    {
        public static IServiceCollection AddMongoService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var hostUrl = configuration["MongoDB:HostUrl"];
            var port = Int32.Parse(configuration["MongoDB:Port"]);
            var databaseName = configuration["MongoDB:Database"];
            var username = configuration["MongoDB:Username"];
            var password = configuration["MongoDB:Password"];

            var settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(hostUrl, port);
            settings.UseTls = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            settings.RetryWrites = false;

            var identity = new MongoInternalIdentity(databaseName, username);
            var evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            var client = new MongoClient(settings);

            services.AddScoped<IMongoClient>((_) => client);

            return services;
        }
    }
}
