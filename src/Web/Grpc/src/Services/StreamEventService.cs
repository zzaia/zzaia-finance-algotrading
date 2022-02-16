using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.Web.Grpc.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Services
{
    public partial class StreamEventService : StreamEventGrpc.StreamEventGrpcBase
    {
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<StreamEventService> _logger;

        public StreamEventService(IDataStreamSource streamSource, ILogger<StreamEventService> logger)
        {
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public override async Task<Empty> RunStreamEvent(IAsyncStreamReader<EventSourceDTO> requestStream, ServerCallContext context)
        {
            Log.RunStream.Received(_logger);
            await foreach (var message in requestStream.ReadAllAsync())
            {
                switch (message.Type)
                {
                    case nameof(OrderBook):
                        var orderbookDTO = message.Content.Unpack<OrderbookDTO>();
                        var orderbook = new OrderBook()
                        {
                            Exchange = Enumeration.FromDisplayName<ExchangeName>(orderbookDTO.ExchangeName),
                        };
                        var eventSource = new EventSource<OrderBook>(orderbook);
                        _streamSource.Publish(eventSource);
                        _logger.LogInformation("### Receiveing event ###");
                        break;
                    default:
                        break;
                }
            }

            return new Empty();
        }
    }
}
