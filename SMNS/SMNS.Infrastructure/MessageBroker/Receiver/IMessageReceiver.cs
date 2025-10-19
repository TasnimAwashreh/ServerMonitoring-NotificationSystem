namespace SMNS.Infrastructure.MessageBroker.Receiver
{
    public interface IMessageReceiver
    {
        Task CreateConnection();
        Task ReceiveMessage(string queue);
        event Action<string>? OnMessageReceived;

    }
}
