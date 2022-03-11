using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.Web.Grpc.Protos;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc.Services
{
    public partial class StreamEventService : StreamEventGrpc.StreamEventGrpcBase
    {
        private readonly IMapper _mapper;
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<StreamEventService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public StreamEventService(IMapper mapper,
                                  IDataStreamSource streamSource,
                                  TelemetryClient telemetryClient,
                                  ILogger<StreamEventService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }
        public override async Task<Empty> RunStreamEvent(IAsyncStreamReader<EventSourceDTO> requestStream, ServerCallContext context)
        {
            Log.RunStream.Received(_logger);
            Log.RunStream.ReceivedAction(_telemetryClient);
            await foreach (var message in requestStream.ReadAllAsync())
            {
                try
                {
                    switch (message.Content.TypeUrl)
                    {
                        case "type.googleapis.com/OrderBookDTO":
                            var eventSource = _mapper.Map<EventSource<OrderBook>>(message);
                            _streamSource.Publish(eventSource);
                            break;
                        default:
                            Log.RunStream.WithBadRequest(_logger, message.Content.TypeUrl);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Log.RunStream.WithException(_logger, ex);
                    continue;
                }
            }

            return new Empty();
        }
    }
}
