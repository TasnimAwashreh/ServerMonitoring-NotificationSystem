using SMNS.Data.Implementations;
using SMNS.Data.Models;

namespace SMNS.Domain
{
    public class ServerStatisticsService : IServerStatisticsService
    {
        private IServerStatisticsRepository _stats;

        public ServerStatisticsService(IServerStatisticsRepository stats)
        {
            _stats = stats;
        }

        public async Task AddStatisticAsync(ServerStatisticsEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            await _stats.InsertStatisticAsync(entity);
        }

        public async Task<List<ServerStatisticsEntity>> GetAllStatisticsAsync()
        {
            return await _stats.GetAllStatisticsAsync();
        }

        public async Task<ServerStatisticsEntity?> GetLatestStatisticAsync()
        {
            return await _stats.GetLatestAsync();
        }

        public async Task DeleteStatisticAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException();
            await _stats.DeleteStatisticAsync(id);
        }

        public async Task UpdateStatisticAsync(ServerStatisticsEntity updatedEntity)
        {
            if (updatedEntity == null)
                throw new ArgumentNullException();
            await _stats.UpdateStatisticAsync();
        }
    }
}
