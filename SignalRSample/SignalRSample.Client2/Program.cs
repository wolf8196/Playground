using JustMyPackages.Hosting;
using SignalRSample.Client;

namespace SignalRSample.Client2
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await HostRunner.RunGenericHost(new Startup(), args, false);
        }
    }
}