using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMNS.Infrastructure.MessageBroker;
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

            var conn = config["Connection"];
            services.AddScoped<CollectStatisticsTimer>();
            services.AddScoped<IMessagePublisher>(p => new MessagePublisher(conn));
            services.AddScoped<IMessageReceiver>(r => new MessageReceiver(conn));
            return services;
        }
    }
}
