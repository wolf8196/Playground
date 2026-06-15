using System.Threading.Tasks;
using JustMyPackages.Hosting;

namespace SignalRSample.Client
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            await HostRunner.RunGenericHost(new Startup(), args, false);
        }
    }
}