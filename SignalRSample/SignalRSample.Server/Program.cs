using System.Threading.Tasks;
using JustMyPackages.Hosting;
using SignalRSample.Server;

internal sealed class Program
{
    private static async Task Main(string[] args)
    {
        await HostRunner.RunWebHost(new Startup(), args, false);
    }
}