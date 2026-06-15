using System;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace SignalRSample.Api.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiClient<TMessageReceiver>(
            this IServiceCollection services,
            Func<IServiceProvider, HubConnection> connectionFactory)
            where TMessageReceiver : class, IMessageReceiver
        {
            services.AddSingleton<IMessageSender, MessageApiClient>(
                sp => ActivatorUtilities.CreateInstance<MessageApiClient>(sp, connectionFactory(sp)));
            services.AddSingleton<IMessageReceiver, TMessageReceiver>();
        }
    }
}