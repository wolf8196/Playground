using System;
using System.Buffers;
using System.IO;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace AzureSamples.Infrastructure.Messaging
{
    internal class ServiceBusPublisher : IPublisher
    {
        private readonly ServiceBusClient client;
        private readonly ServiceBusSender sender;

        public ServiceBusPublisher(ServiceBusPublisherOptions options)
        {
            client = new ServiceBusClient(options.ConnectionString);
            sender = client.CreateSender(options.QueueOrTopicName);
        }

        public async Task PublishAsync<T>(T obj, CancellationToken token)
        {
            using var data = Serialize(obj);
            await sender.SendMessageAsync(new ServiceBusMessage(BinaryData.FromStream(data, MediaTypeNames.Application.Json)), token);
        }

        public async ValueTask DisposeAsync()
        {
            await client.DisposeAsync();
            await sender.DisposeAsync();
        }

        private static MemoryStream Serialize<T>(T obj)
        {
            var stream = new MemoryStream();
            JsonSerializer.Serialize(stream, obj, JsonSerializerOptions.Web);
            stream.Position = 0;
            return stream;
        }
    }
}