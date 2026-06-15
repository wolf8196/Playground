using System.Threading.Tasks;
using Serilog;
using SignalRSample.Api;

namespace SignalRSample.Client
{
    internal sealed class MessageReceiver : IMessageReceiver
    {
        private readonly ILogger<MessageReceiver> logger;

        public MessageReceiver(ILogger<MessageReceiver> logger)
        {
            this.logger = logger;
        }

        public Task ReceiveMessageAsync(MessageDto message)
        {
            logger.Information("Received message from {User}: {Message}", message.UserName, message.Text);
            return Task.CompletedTask;
        }
    }
}