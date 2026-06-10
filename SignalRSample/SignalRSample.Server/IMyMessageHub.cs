using System.Threading.Tasks;

namespace SignalRSample.Server
{
    public interface IMyMessageHub
    {
        Task ReceiveMessage(string user, string message);
    }
}