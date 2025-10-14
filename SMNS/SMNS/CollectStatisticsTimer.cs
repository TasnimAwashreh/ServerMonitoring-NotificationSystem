using Microsoft.Extensions.Configuration;
using SMNS.Infrastructure.MessageBroker.Publisher;
using SMNS.Infrastructure.Monitoring;

namespace SMNS.App
{
    public class CollectStatisticsTimer
    {
        private readonly System.Timers.Timer _timer;
        private IMessagePublisher _publisher;
        private string _serverIdentifier;

        public CollectStatisticsTimer(IMessagePublisher publisher, IConfiguration config)
        {
            _serverIdentifier = config["ServerStatisticsConfig:ServerIdentifier"] ?? "Unknown";
            int samplingIntervalSeconds = int.Parse(config["ServerStatisticsConfig:SamplingIntervalSeconds"] ?? "5");
            _publisher = publisher;

            _timer = new System.Timers.Timer(samplingIntervalSeconds * 1000);
            _timer.Elapsed += OnTimedStatisticsCollecting;
            _timer.Enabled = true;
        }

        private async void OnTimedStatisticsCollecting(object? sender, System.Timers.ElapsedEventArgs e)
        {
            var serverStats = ServerStatisticsCollector.CollectStatistics();
            string message = $"({_serverIdentifier})  [{serverStats.Timestamp}] CPU:{serverStats.CpuUsage} MEMORY:{serverStats.AvailableMemory}";
            try
            {
                await _publisher.PublishMessage(_serverIdentifier, message);
            }
            catch (InvalidOperationException) { Console.WriteLine("RabbitMQ's connection or channel is not initialized"); }
            Console.WriteLine("- SENT: " + message);
        }
    }
}
