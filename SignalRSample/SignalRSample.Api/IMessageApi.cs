using System.Threading.Tasks;

namespace SignalRSample.Api
{
    public interface IMessageReceiver
    {
        Task ReceiveMessageAsync(MessageDto message);
    }

    public interface IMessageSender
    {
        Task SendMessageAsync(MessageDto message);
    }
}