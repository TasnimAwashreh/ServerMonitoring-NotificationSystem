using MongoDB.Driver;
using SMNS.Data.Models;

namespace SMNS.Data
{
    public class MongoDBClient
    {
        private IMongoDatabase _db;

        public MongoDBClient(string conn, string dbName)
        {
            var client = new MongoClient(conn);
            _db = client.GetDatabase(dbName);
        }

        public IMongoCollection<ServerStatisticsEntity> ServerStatistics =>
        _db.GetCollection<ServerStatisticsEntity>("ServerStatistics");
    }
}
