using SMNS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMNS.Domain
{
    public interface IServerStatisticsService
    {
        Task AddStatisticAsync(ServerStatisticsEntity entity);
        Task<List<ServerStatisticsEntity>> GetAllStatisticsAsync();
        Task<ServerStatisticsEntity?> GetLatestStatisticAsync();
        Task DeleteStatisticAsync(string id);
        Task UpdateStatisticAsync(ServerStatisticsEntity updatedEntity);
    }
}
