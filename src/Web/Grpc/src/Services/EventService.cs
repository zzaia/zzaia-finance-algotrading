using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.Web.Grpc.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Services
{
    public partial class EventService : EventGrpc.EventGrpcBase
    {
        private readonly IMapper _mapper;
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<StreamEventService> _logger;

        public EventService(IMapper mapper,
                                  IDataStreamSource streamSource,
                                  ILogger<StreamEventService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
        }

        public override async Task<Empty> RunEvent(EventSourceDTO request, ServerCallContext context)
        {
            switch (request.Content.TypeUrl)
            {
                case "type.googleapis.com/OrderBookDTO":
                    var eventSource = _mapper.Map<EventSource<OrderBook>>(request);
                    _streamSource.Publish(eventSource);
                    return await Task.FromResult(new Empty());
                default:
                    return await Task.FromResult(new Empty());
            }
        }
    }
}
