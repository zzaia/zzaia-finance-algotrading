using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using Zzaia.Finance.EventManager;
using Zzaia.Finance.Web.Grpc.Protos;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Zzaia.Finance.Web.Grpc.Services
{
    public partial class EventService : EventGrpc.EventGrpcBase
    {
        private readonly IMapper _mapper;
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<StreamEventService> _logger;
        private readonly TelemetryClient _telemetryClient;

        public EventService(IMapper mapper,
                            IDataStreamSource streamSource,
                            TelemetryClient telemetryClient,
                            ILogger<StreamEventService> logger)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        public override async Task<Empty> RunEvent(EventSourceDTO request, ServerCallContext context)
        {
            Log.Run.Received(_logger);
            Log.Run.ReceivedAction(_telemetryClient);
            try
            {
                switch (request.Content.TypeUrl)
                {
                    case "type.googleapis.com/OrderBookDTO":
                        var eventSource = _mapper.Map<EventSource<OrderBook>>(request);
                        _streamSource.Publish(eventSource);
                        return await Task.FromResult(new Empty());
                    default:
                        Log.Run.WithBadRequest(_logger, request.Content.TypeUrl);
                        return await Task.FromResult(new Empty());
                }
            }
            catch (Exception ex)
            {
                Log.Run.WithException(_logger, ex);
                return await Task.FromResult(new Empty());
            }
        }
    }
}
