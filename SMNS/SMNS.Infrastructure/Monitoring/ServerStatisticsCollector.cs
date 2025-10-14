using System.Diagnostics;
using System.Timers;

namespace SMNS.Infrastructure.Monitoring
{
    public static class ServerStatisticsCollector
    {
        public static PerformanceCounter cpuCounter;
        public static PerformanceCounter ramCounter;
        public static PerformanceCounter memoryCounter;

        static ServerStatisticsCollector()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            memoryCounter = new PerformanceCounter("Process", "Working Set", 
                Process.GetCurrentProcess().ProcessName);
        }

        public static ServerStatistics CollectStatistics()
        {
            var cpuUsage = cpuCounter.NextValue();
            var availableRam = ramCounter.NextValue();
            var processMemoryUsage = memoryCounter.NextValue() / (1024 * 1024); 

            return new ServerStatistics
            {
                MemoryUsage = processMemoryUsage,
                AvailableMemory = availableRam,
                CpuUsage = cpuUsage,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
