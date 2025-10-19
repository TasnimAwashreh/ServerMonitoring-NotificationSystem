using SMNS.App;
using SMNS.Domain;
using SMNS.Infrastructure.MessageBroker.Receiver;
using SMNS.Infrastructure.AnomalyDetection;
using Microsoft.AspNetCore.SignalR;


public class Program
{
    public static async Task Main()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddConfiguration();
                services.AddSignalR();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://localhost:5000");
                webBuilder.Configure(app =>
                {
                    app.UseStaticFiles();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<NotificationHub>("/stats");
                    });
                });
            });
        var host = builder.Build();

        var config = host.Services.GetRequiredService<IConfiguration>();
        var receiver = host.Services.GetRequiredService<IMessageReceiver>();
        var statisticsService = host.Services.GetRequiredService<IServerStatisticsService>();
        var statsTimer = host.Services.GetRequiredService<StastisticsPublisherTimer>();
        var notificationService = host.Services.GetRequiredService<INotificationService>();

        var serverIdentifier = config["ServerStatisticsConfig:ServerIdentifier"] ?? "Unknown";

        var signalRTask = host.RunAsync();
        var processor = new StatisticsProcessor(receiver, statisticsService, notificationService);
        await processor.RunReceiverAsync(serverIdentifier);

        await Task.WhenAll(signalRTask);
    }
}