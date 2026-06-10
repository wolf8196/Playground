using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Server
{
    public class MyHub : Hub<IMyMessageHub>
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.Others.ReceiveMessage(user, message);
        }
    }
}
