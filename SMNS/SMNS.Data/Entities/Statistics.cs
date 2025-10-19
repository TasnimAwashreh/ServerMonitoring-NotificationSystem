using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SMNS.Data.Models
{
    public class ServerStatisticsEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("memory_usage")]
        public double MemoryUsage { get; set; }

        [BsonElement("available_memory")]
        public double AvailableMemory { get; set; }

        [BsonElement("cpu_usage")]
        public double CpuUsage { get; set; }

        [BsonElement("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
