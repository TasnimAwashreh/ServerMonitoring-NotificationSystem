using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMNS.Infrastructure.MessageBroker.Publisher
{
    public interface IMessagePublisher
    {
        Task CreateConnection();
        Task PublishMessage(string queue, string message);
    }
}
