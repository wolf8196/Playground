using System;
using System.Threading.Tasks;
using AzureSamples.Api.Options;
using AzureSamples.Infrastructure;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureSamples.Api
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = FunctionsApplication.CreateBuilder(args);

            builder.Services.AddSingleton(
                static sp => sp.GetRequiredService<IConfiguration>().Get<ApiOptions>()
                    ?? throw new ArgumentNullException(nameof(ApiOptions)));

            builder.Services.AddInfrastructure(sp =>
            {
                var config = sp.GetRequiredService<ApiOptions>();
                return new InfrastructureOptions
                {
                    DbConnectionString = config.DbConnectionString,
                    EventPublishOptions = new Infrastructure.Messaging.ServiceBusPublisherOptions
                    {
                        ConnectionString = config.ServiceBusConnectionString,
                        QueueOrTopicName = config.EventsQueue
                    }
                };
            });

            var host = builder.Build();
            await host.RunAsync();
        }
    }
}