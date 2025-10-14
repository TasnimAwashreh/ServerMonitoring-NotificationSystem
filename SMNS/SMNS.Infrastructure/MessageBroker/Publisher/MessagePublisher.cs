using RabbitMQ.Client;
using System.Text;

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

        public async Task PublishMessage(string queue, string message)
        {
            if (connection == null || channel == null)
                throw new InvalidOperationException("RabbitMQ's connection or channel is not initialized");
            await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            var body = Encoding.UTF8.GetBytes(message);

            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queue, body: body);
        }
    }
}
