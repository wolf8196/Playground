using System;
using AzureSamples.Core;
using AzureSamples.Infrastructure.Messaging;
using AzureSamples.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace AzureSamples.Infrastructure
{
    public static class InfrastrcutureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Func<IServiceProvider, InfrastructureOptions> options)
        {
            services.AddDbContext<AzureSamplesDbContext>((sp, opts) =>
            {
                opts.UseSqlServer(options(sp).DbConnectionString);
            });

            services.AddScoped<IEventProvider, EventProvider>();

            services.AddSingleton<IPublisher>(sp => new ServiceBusPublisher(options(sp).EventPublishOptions));
            return services;
        }
    }
}