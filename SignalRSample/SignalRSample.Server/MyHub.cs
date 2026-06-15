using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Api;

namespace SignalRSample.Server
{
    public class MyHub : Hub<IMessageReceiver>, IMessageSender
    {
        public async Task SendMessageAsync(MessageDto message)
        {
            await Clients.Others.ReceiveMessageAsync(message);
        }
    }
}