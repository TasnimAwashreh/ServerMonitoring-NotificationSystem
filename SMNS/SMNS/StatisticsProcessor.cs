using SMNS.Data.Models;
using SMNS.Domain;
using SMNS.Infrastructure.MessageBroker.Receiver;
using SMNS.Infrastructure.Models;
using System.Text.Json;

namespace SMNS.App
{
    public class StatisticsProcessor
    {
        private IMessageReceiver _receiver;
        private IServerStatisticsService _statsService;
        private INotificationService _notificationService;
        private IConfiguration _config;
        private ServerStatistics _previousStats;

        public StatisticsProcessor(IMessageReceiver receiver, IServerStatisticsService statsService, INotificationService notificationService)
        {
            _receiver = receiver;
            _statsService = statsService;
            _notificationService = notificationService;
            _receiver.OnMessageReceived += ReceivedMessageHandler;
            _previousStats = new ServerStatistics { MemoryUsage = 0, AvailableMemory = 0, CpuUsage = 0, Timestamp = DateTime.UtcNow };
        }

        private async void ReceivedMessageHandler(string message)
        {
            try
            {
                ServerStatistics? stats = JsonSerializer.Deserialize<ServerStatistics>(message);

                if (stats == null)
                    throw new NullReferenceException();
                {
                    var entity = new ServerStatisticsEntity
                    {
                        CpuUsage = stats.CpuUsage,
                        MemoryUsage = stats.MemoryUsage,
                        AvailableMemory = stats.AvailableMemory,
                        Timestamp = stats.Timestamp,
                    };

                    await _statsService.AddStatisticAsync(entity);
                    await _notificationService.Notify(_previousStats, stats);
                    _previousStats = stats;
                }
            }
            catch (NullReferenceException ex) { Console.WriteLine($"Error deserializing message: {ex}"); }
            catch (Exception ex) { Console.WriteLine($"Error saving message: {ex}"); }
        }

        public async Task RunReceiverAsync(string queue)
        {
            await _receiver.CreateConnection();
            var receiveTask = Task.Run(async () =>
            {
                await _receiver.ReceiveMessage(queue);
            });
            await Task.Delay(-1);
        }
    }
}
