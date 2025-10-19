using Microsoft.AspNetCore.SignalR;

namespace SMNS.Infrastructure.AnomalyDetection
{
    public class NotificationHub : Hub<INotificationHub>
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendMessage(message);
        }
    }
}
