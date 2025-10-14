using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMNS.Infrastructure.MessageBroker.Receiver
{
    public interface IMessageReceiver
    {
        Task CreateConnection();
        Task ReceiveMessage(string queue);

    }
}
