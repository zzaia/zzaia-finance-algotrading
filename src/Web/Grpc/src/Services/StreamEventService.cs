using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.Web.Grpc.Protos;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Services
{
    public partial class StreamEventService : StreamEventGrpc.StreamEventGrpcBase
    {
        private readonly IDataStreamSource _streamSource;

        public StreamEventService(IDataStreamSource streamSource)
        {
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
        }
        public override async Task<Empty> RunStreamEvent(IAsyncStreamReader<EventSourceDTO> requestStream, ServerCallContext context)
        {
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
                        break;
                    default:
                        break;
                }
            }

            return new Empty();
        }
    }
}
