using System;
using Flurl;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SignalRSample.HubApi
{
    public static class HubApiServiceCollectionExtensions
    {
        public static IServiceCollection AddHubConnection(
            this IServiceCollection services,
            string route,
            Func<IServiceProvider, string> serviceUrl)
        {
            services.TryAddSingleton<IRetryPolicy>(
                sp => new ExponentialBackoffWithJitter(TimeSpan.FromSeconds(2), TimeSpan.FromMinutes(1)));
            services.AddKeyedSingleton(route, (sp, key) =>
            {
                return new HubConnectionBuilder()
                    .WithUrl(Url.Combine(serviceUrl(sp), key!.ToString()))
                    .WithAutomaticReconnect(sp.GetRequiredService<IRetryPolicy>())
                    .Build();
            });
            services.AddSingleton(sp => sp.GetRequiredKeyedService<HubConnection>(route));
            return services;
        }

        public static IServiceCollection AddHubConnectionService(this IServiceCollection services)
        {
            services.AddHostedService<HubConnectionService>();
            return services;
        }

        public static IServiceCollection AddHubApiClient<TClient, TSender, TReceiver>(
            this IServiceCollection services,
            string route)
            where TClient : HubApiClient<TSender, TReceiver>, TSender, TReceiver
            where TSender : class
            where TReceiver : class
        {
            services.AddSingleton(
                sp => ActivatorUtilities.CreateInstance<TClient>(sp, sp.GetRequiredKeyedService<HubConnection>(route)));
            services.AddSingleton<TReceiver>(sp => sp.GetRequiredService<TClient>());
            services.AddSingleton<TSender>(sp => sp.GetRequiredService<TClient>());

            return services;
        }
    }
}