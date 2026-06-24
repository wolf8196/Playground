using System;
using Microsoft.Extensions.DependencyInjection;
using SignalRSample.Api.Client.Events;
using SignalRSample.Api.Client.Messages;
using SignalRSample.HubApi;

namespace SignalRSample.Api.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessageApiClient<TListener>(
            this IServiceCollection services,
            Func<IServiceProvider, string> serviceUrl)
            where TListener : HubListener, IMessageReceiver
        {
            services.AddHubConnection(Routes.MessageHubRoute, serviceUrl);
            services.AddHubConnectionService();
            services.AddHubApiClient<MessageApiClient, IMessageSender>(Routes.MessageHubRoute);
            services.AddHubListener<TListener, IMessageReceiver>(Routes.MessageHubRoute);
        }

        public static void AddEventApiClient<TListener>(
            this IServiceCollection services,
            Func<IServiceProvider, string> serviceUrl)
            where TListener : HubListener, IEventReceiver
        {
            services.AddHubConnection(Routes.EventHubRoute, serviceUrl);
            services.AddHubConnectionService();
            services.AddHubApiClient<EventApiClient, IEventSender>(Routes.EventHubRoute);
            services.AddHubListener<TListener, IEventReceiver>(Routes.EventHubRoute);
        }
    }
}