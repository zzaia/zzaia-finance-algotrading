using Dapr.AppCallback.Autogen.Grpc.v1;
using Dapr.Client;
using Dapr.Client.Autogen.Grpc.v1;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MarketIntelligency.Application.DataEventManager.Services
{
    public class DataEventManagerService : AppCallback.AppCallbackBase
    {
        private readonly ILogger<DataEventManagerService> _logger;
        private readonly DaprClient _daprClient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="daprClient"></param>
        /// <param name="logger"></param>
        public DataEventManagerService(DaprClient daprClient, ILogger<DataEventManagerService> logger)
        {
            _daprClient = daprClient;
            _logger = logger;
        }

        public override async Task<InvokeResponse> OnInvoke(InvokeRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"Method {request.Method}");

            var response = new InvokeResponse();
            switch (request.Method)
            {
                case "orderbook":
                    var input = request.Data.Unpack<EventSource<OrderBook>>();
                    var output = await Task.FromResult(new Response());
                    response.Data = Any.Pack(output);
                    break;
                default:
                    _logger.LogError("Method not supported");
                    break;
            }
            return response;
        }

        /// <inheritdoc/>
        public override Task<ListInputBindingsResponse> ListInputBindings(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListInputBindingsResponse());
        }

        /// <inheritdoc/>
        public override Task<ListTopicSubscriptionsResponse> ListTopicSubscriptions(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new ListTopicSubscriptionsResponse());
        }
    }
}
