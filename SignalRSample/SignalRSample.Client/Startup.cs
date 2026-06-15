using JustMyPackages.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRSample.Api;
using SignalRSample.Api.Client;

namespace SignalRSample.Client
{
    internal sealed class Startup : IGenericHostStartup
    {
        public void ConfigureHost(IHostBuilder host)
        {
            host.ConfigureMyHost();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHostedService<Service>();
            services.AddSingleton(sp =>
            {
                return new HubConnectionBuilder()
                    .WithUrl($"http://localhost:4321/{Routes.MyHubRoute}")
                    // .WithAutomaticReconnect()
                    .Build();
            });
            services.AddApiClient<MessageReceiver>(sp =>
            {
                return sp.GetRequiredService<HubConnection>();
            });

            services.AddHostedService<ConnectionManager>();
        }
    }
}