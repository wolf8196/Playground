using System;
using JustMyPackages.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRSample.Api.Client;
using SignalRSample.Client.HubListeners;
using SignalRSample.Client.Services;

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
            // services.AddHostedService<MessageService>();
            services.AddHostedService(sp => (EventService)sp.GetRequiredService<IEventService>());
            services.AddSingleton<IEventService, EventService>();
            services.AddMessageApiClient<MessageReceiver>(sp => sp.GetRequiredService<ClientOptions>().ServiceUrl);
            services.AddEventApiClient<EventReceiver>(sp => sp.GetRequiredService<ClientOptions>().ServiceUrl);
        }
    }
}