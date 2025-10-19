namespace SMNS.Infrastructure.AnomalyDetection
{
    public interface INotificationHub
    {
        Task SendMessage(string message);
    }
}
