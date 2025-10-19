using SMNS.Infrastructure.Models;

namespace SMNS.Domain
{
    public interface INotificationService
    {
        Task Notify(ServerStatistics previousStats, ServerStatistics currrentStats);
    }
}
