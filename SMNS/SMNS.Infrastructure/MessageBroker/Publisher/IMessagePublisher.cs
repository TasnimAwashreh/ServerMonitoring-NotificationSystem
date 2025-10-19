using SMNS.Infrastructure.Models;

namespace SMNS.Infrastructure.MessageBroker.Publisher
{
    public interface IMessagePublisher
    {
        Task CreateConnection();
        Task PublishMessage(string queue, ServerStatistics stats);
    }
}
