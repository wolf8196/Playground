using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Server
{
    public class MyHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
