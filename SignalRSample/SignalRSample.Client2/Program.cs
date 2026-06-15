using System.Threading.Tasks;
using JustMyPackages.Hosting;
using SignalRSample.Client;

namespace SignalRSample.Client2
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            await HostRunner.RunGenericHost(new Startup(), args, false);
        }
    }
}