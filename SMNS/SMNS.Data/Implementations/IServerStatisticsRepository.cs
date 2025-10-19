using SMNS.Data.Models;

namespace SMNS.Data.Implementations
{
    public interface IServerStatisticsRepository
    {
        Task InsertStatisticAsync(ServerStatisticsEntity entity);
        Task<List<ServerStatisticsEntity>> GetAllStatisticsAsync();
        Task<ServerStatisticsEntity?> GetLatestAsync();
        Task DeleteStatisticAsync(string id);
        Task UpdateStatisticAsync();

    }
}
