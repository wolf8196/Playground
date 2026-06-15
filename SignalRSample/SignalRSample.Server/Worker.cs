using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SignalRSample.Server
{
    public class Worker : BackgroundService
    {
        //private readonly IHubContext<MyHub, IMessageReceiver> hubContext;

        //public Worker(IHubContext<MyHub, IMessageReceiver> hubContext)
        //{
        //    this.hubContext = hubContext;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //await hubContext.Clients.All.ReceiveMessageAsync(new MessageDto
                //{
                //    UserName = "Server",
                //    Text = $"Server check. Date: {DateTime.Now}",
                //});

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
