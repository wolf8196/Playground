using System;
using JustMyPackages.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRSample.Api;
using SignalRSample.Api.Client;
using SignalRSample.Client.HubClients;

namespace SignalRSample.Client
{
    public sealed class Startup : IGenericHostStartup
    {
        public void ConfigureHost(IHostBuilder host)
        {
            host.ConfigureMyHost();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(
                sp => sp.GetRequiredService<IConfiguration>().Get<ClientOptions>()
                    ?? throw new ArgumentNullException(nameof(ClientOptions)));
            services.AddHostedService<Service>();
            services.AddMessageApiClient<MessageApiReceiver>(Routes.MyHubRoute, sp => sp.GetRequiredService<ClientOptions>().ServiceUrl);
        }
    }
}