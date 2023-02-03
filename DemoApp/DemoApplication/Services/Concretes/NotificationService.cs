using DemoApplication.Areas.Admin.Hubs;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.SignalR;

namespace DemoApplication.Services.Concretes
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<AlertHub> _alertHub;
        public NotificationService(IHubContext<AlertHub> alertHub)
        {
            _alertHub = alertHub;
        }

        public async Task SentOrderCreatedToAdmin()
        {
            await _alertHub.Clients.All.SendAsync("Notify", "hello ceyhun");

        }
    }
}
