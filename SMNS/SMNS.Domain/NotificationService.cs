using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using SMNS.Infrastructure.AnomalyDetection;
using SMNS.Infrastructure.Models;
using System.Text;

namespace SMNS.Domain
{
    public class NotificationService : INotificationService
    {
        private IHubContext<NotificationHub, INotificationHub> _messageHub;
        private IConfiguration _config;

        double _memoryUsageAnomalyThresholdPercentage;
        double _cpuUsageAnomalyThresholdPercentage;
        double _memoryUsageThresholdPercentage;
        double _cpuUsageThresholdPercentage;

        public NotificationService(IConfiguration config, IHubContext<NotificationHub, INotificationHub> messageHub)
        {
            _config = config;
            _messageHub = messageHub;
            _memoryUsageAnomalyThresholdPercentage = double.Parse(_config["AnomalyDetectionConfig:MemoryUsageAnomalyThresholdPercentage"] ?? "0.0");
            _cpuUsageAnomalyThresholdPercentage = double.Parse(_config["AnomalyDetectionConfig:CpuUsageAnomalyThresholdPercentage"] ?? "0.0");
            _memoryUsageThresholdPercentage = double.Parse(_config["AnomalyDetectionConfig:MemoryUsageThresholdPercentage"] ?? "0.0");
            _cpuUsageThresholdPercentage = double.Parse(_config["AnomalyDetectionConfig:CpuUsageThresholdPercentage"] ?? "0.0");
        }

        public async Task Notify(ServerStatistics previousStats, ServerStatistics currrentStats)
        {
            var strBuilder = new StringBuilder();
            if (currrentStats.MemoryUsage > (previousStats.MemoryUsage * (1.0 + _memoryUsageAnomalyThresholdPercentage)))
                strBuilder.AppendLine($"High Memory Usage Anomaly Alert!");
            if (currrentStats.CpuUsage > (previousStats.CpuUsage * (1.0 + _cpuUsageAnomalyThresholdPercentage)))
                strBuilder.AppendLine($"High CPU Usage Anomaly Alert!");
            if ((currrentStats.MemoryUsage / (currrentStats.MemoryUsage + currrentStats.AvailableMemory)) > _memoryUsageThresholdPercentage)
                strBuilder.AppendLine($"Memory High Usage Alert!");
            if (currrentStats.CpuUsage > _cpuUsageThresholdPercentage)
                strBuilder.AppendLine($"CPU High Usage Alert!");
            if (strBuilder.ToString() == "")
                strBuilder.AppendLine("No errors");
            await _messageHub.Clients.All.SendMessage(strBuilder.ToString() + "\n");
        }
    }
}
