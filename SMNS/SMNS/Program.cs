using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMNS.App;
using SMNS.Infrastructure.MessageBroker.Publisher;
using SMNS.Infrastructure.MessageBroker.Receiver;

public class Program
{
    public static async Task Main()
    {
        var services = new ServiceCollection();
        services.AddConfiguration();
        var serviceProvider = services.BuildServiceProvider();

        var config = serviceProvider.GetRequiredService<IConfiguration>();
        var serverIdentifier =  config["ServerStatisticsConfig:ServerIdentifier"] ?? "Unknown";
        var statisticsTimer = serviceProvider.GetRequiredService<CollectStatisticsTimer>();
        
        var publisher = serviceProvider.GetRequiredService<IMessagePublisher>();
        await publisher.CreateConnection();

        var receiver = serviceProvider.GetRequiredService<IMessageReceiver>();
        await RunReceiver(receiver, serverIdentifier);
    }

    public static async Task RunReceiver(IMessageReceiver receiver, string queue)
    {
        await receiver.CreateConnection();
        var receiveTask = Task.Run(async () =>
        {
            await receiver.ReceiveMessage(queue);
        });
        await Task.Delay(-1);
    }
}