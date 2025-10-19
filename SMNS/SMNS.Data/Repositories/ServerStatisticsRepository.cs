using MongoDB.Driver;
using SMNS.Data.Implementations;
using SMNS.Data.Models;

namespace SMNS.Data.Repositories
{
    public class ServerStatisticsRepository : IServerStatisticsRepository
    {
        private MongoDBClient _client;

        public ServerStatisticsRepository(MongoDBClient client)
        {
            this._client = client;
        }

        public async Task InsertStatisticAsync(ServerStatisticsEntity entity)
        {
            await _client.ServerStatistics.InsertOneAsync(entity);
        }

        public async Task<List<ServerStatisticsEntity>> GetAllStatisticsAsync()
        {
            return await _client.ServerStatistics.Find(_ => true).ToListAsync();
        }

        public async Task<ServerStatisticsEntity?> GetLatestAsync()
        {
            return await _client.ServerStatistics
                .Find(_ => true)
                .SortByDescending(s => s.Timestamp)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteStatisticAsync(string id)
        {
            await _client.ServerStatistics.DeleteOneAsync(id);
        }

        public async Task UpdateStatisticAsync()
        {
            throw new NotImplementedException();
        }
    }
}
