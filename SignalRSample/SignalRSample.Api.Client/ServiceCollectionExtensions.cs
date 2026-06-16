using System;
using Microsoft.Extensions.DependencyInjection;
using SignalRSample.HubApi;

namespace SignalRSample.Api.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageApiClient<TReceiverImpl>(
            this IServiceCollection services,
            string route,
            Func<IServiceProvider, string> serviceUrl)
            where TReceiverImpl : MessageApiClient
        {
            services.AddHubConnection(route, serviceUrl);
            services.AddHubConnectionService();
            services.AddHubApiClient<TReceiverImpl, IMessageSender, IMessageReceiver>(route);
        }
    }
}