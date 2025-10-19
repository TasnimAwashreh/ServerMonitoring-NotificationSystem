using RabbitMQ.Client;
using SMNS.Infrastructure.Models;
using System.Text;
using System.Text.Json;

namespace SMNS.Infrastructure.MessageBroker.Publisher
{
    public class MessagePublisher : IMessagePublisher
    {
        ConnectionFactory factory;
        IConnection? connection;
        IChannel? channel;

        public MessagePublisher(string hostName)
        {
            factory = new ConnectionFactory { HostName = hostName };
        }

        public async Task CreateConnection()
        {
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();
        }

        public async Task PublishMessage(string queue, ServerStatistics stats)
        {
            if (connection == null || channel == null)
                throw new InvalidOperationException();
            
            await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

            string serializedMessage = JsonSerializer.Serialize(stats);
            var body = Encoding.UTF8.GetBytes(serializedMessage);
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, body: body);
        }
    }
}
