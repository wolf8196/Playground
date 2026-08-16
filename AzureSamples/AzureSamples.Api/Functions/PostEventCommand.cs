using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AzureSamples.Api.Dtos;
using AzureSamples.Infrastructure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace AzureSamples.Api.Functions
{
    public class PostEventCommand
    {
        private readonly IPublisher publisher;

        public PostEventCommand(IPublisher publisher)
        {
            this.publisher = publisher;
        }

        [OpenApiOperation(operationId: "post-event", tags: ["Events"], Summary = "Post event")]
        [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(EventDto), Description = "Event")]
        [Function(nameof(PostEventCommand))]
        public async Task<HttpResponseData> HandleAsync(
            [HttpTrigger("post", Route = "events")] HttpRequestData req,
            [FromBody] EventDto dto,
            FunctionContext context,
            CancellationToken cancellationToken)
        {
            await publisher.PublishAsync(dto, cancellationToken);
            var resp = HttpResponseData.CreateResponse(req);
            resp.StatusCode = HttpStatusCode.OK;
            return resp;
        }
    }
}