using SMNS.Data;
using SMNS.Data.Implementations;
using SMNS.Data.Repositories;
using SMNS.Domain;
using SMNS.Infrastructure.MessageBroker.Publisher;
using SMNS.Infrastructure.MessageBroker.Receiver;

namespace SMNS.App
{
    public static class Configuration
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            services.AddSingleton<IConfiguration>(config);

            //RABBITMQ
            string messageBrokerConn = config["ConnectionStrings:RabbitMQ"];
            string serverIdentifier = config["ServerStatisticsConfig:ServerIdentifier"] ?? "Unknown";
            int samplingIntervalSeconds = int.Parse(config["ServerStatisticsConfig:SamplingIntervalSeconds"] ?? "5");

            services.AddSingleton<IMessagePublisher>(p => new MessagePublisher(messageBrokerConn));
            services.AddSingleton<IMessageReceiver>(r => new MessageReceiver(messageBrokerConn));

            services.AddSingleton(sp =>
            {
                var publisher = sp.GetRequiredService<IMessagePublisher>();
                return new StastisticsPublisherTimer(publisher, serverIdentifier, samplingIntervalSeconds);
            });

            //MONGODB
            var dbConn = config["MongoDB:ConnectionString"];
            var dbName = config["MongoDB:DatabaseName"];

            services.AddSingleton<MongoDBClient>(c => new MongoDBClient(dbConn, dbName));
            services.AddSingleton<IServerStatisticsRepository, ServerStatisticsRepository>();
            services.AddSingleton<IServerStatisticsService, ServerStatisticsService>();

            //SignalR
            services.AddSignalR();
            services.AddSingleton<INotificationService, NotificationService>();
            return services;
        }
    }
}
