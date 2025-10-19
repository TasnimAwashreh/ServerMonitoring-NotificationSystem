using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace SMNS.Infrastructure.MessageBroker.Receiver
{
    public class MessageReceiver : IMessageReceiver
    {
        ConnectionFactory factory;
        IConnection? connection;
        IChannel? channel;

        public event Action<string>? OnMessageReceived;


        public MessageReceiver(string hostName)
        {
            factory = new ConnectionFactory { HostName = hostName };
        }

        public async Task CreateConnection()
        {
            connection = await factory.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();
        }

        public async Task ReceiveMessage(string queue)
        {
            if (connection == null || channel == null)
                throw new InvalidOperationException();

            await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                OnMessageReceived?.Invoke(message);
                return Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(queue, autoAck: true, consumer: consumer);
        }
    }
}
