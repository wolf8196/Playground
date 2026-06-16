using System;
using System.Threading.Tasks;
using JustMyPackages.Hosting;
using Microsoft.Extensions.Hosting;
using SignalRSample.Server;

internal sealed class Program
{
    public static TimeSpan Delay;

    private static async Task Main(string[] args)
    {
        do
        {
            IHost host = HostCreator.Create(new Startup(), args);
            await host.RunAsync();
            await Task.Delay(Delay);
        }
        while (true);
    }
}