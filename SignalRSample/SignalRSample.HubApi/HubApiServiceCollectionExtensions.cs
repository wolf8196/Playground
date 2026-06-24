using System;
using Flurl;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace SignalRSample.HubApi
{
    public static class HubApiServiceCollectionExtensions
    {
        public static IServiceCollection AddHubConnection(
            this IServiceCollection services,
            string route,
            Func<IServiceProvider, string> serviceUrl)
        {
            services.AddKeyedSingleton(route, (sp, key) =>
            {
                var url = Url.Combine(serviceUrl(sp), key!.ToString());
                var connection = new HubConnectionBuilder()
                    .WithUrl(url, HttpTransportType.WebSockets)
                    .WithAutomaticReconnect(
                        new ExponentialBackoffWithJitter(
                            url,
                            TimeSpan.FromSeconds(2),
                            TimeSpan.FromMinutes(1),
                            logger: sp.GetRequiredService<ILogger<ExponentialBackoffWithJitter>>()))
                    .Build();
                return new NamedHubConnection
                {
                    Route = route,
                    Url = url,
                    Connection = connection
                };
            });
            services.AddKeyedSingleton(route, (sp, key) => sp.GetRequiredKeyedService<NamedHubConnection>(route).Connection);
            services.AddSingleton(sp => sp.GetRequiredKeyedService<NamedHubConnection>(route));
            return services;
        }

        public static IServiceCollection AddHubConnectionService(this IServiceCollection services)
        {
            services.AddHostedService<HubConnectionService>();
            return services;
        }

        public static IServiceCollection AddHubApiClient<TClient, TSender>(
            this IServiceCollection services,
            string route)
            where TClient : HubApiClient<TSender>, TSender
            where TSender : class
        {
            services.AddSingleton(
                sp => ActivatorUtilities.CreateInstance<TClient>(sp, sp.GetRequiredKeyedService<HubConnection>(route)));
            services.AddSingleton<TSender>(sp => sp.GetRequiredService<TClient>());

            return services;
        }

        public static IServiceCollection AddHubListener<TListener, TReceiver>(
            this IServiceCollection services,
            string route)
            where TListener : HubListener, TReceiver
            where TReceiver : class
        {
            services.AddSingleton(
                sp => ActivatorUtilities.CreateInstance<TListener>(sp, sp.GetRequiredKeyedService<HubConnection>(route)));
            services.AddSingleton<TReceiver>(sp => sp.GetRequiredService<TListener>());
            services.AddSingleton<HubListener>(sp => sp.GetRequiredService<TListener>());

            return services;
        }
    }
}