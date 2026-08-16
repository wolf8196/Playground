using System;
using System.Threading;
using System.Threading.Tasks;

namespace AzureSamples.Infrastructure.Messaging
{
    public interface IPublisher : IAsyncDisposable
    {
        Task PublishAsync<T>(T obj, CancellationToken token);
    }
}