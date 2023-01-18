using AutoMapper;
using Grpc.Core;
using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using Zzaia.Finance.EventManager;
using Zzaia.Finance.Web.Grpc.Protos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zzaia.Finance.Web.Grpc
{
    public class StreamEventCommunicationHandler : BackgroundService
    {
        private readonly IMapper _mapper;
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<StreamEventCommunicationHandler> _logger;
        private readonly StreamEventGrpc.StreamEventGrpcClient _client;
        private IClientStreamWriter<EventSourceDTO> _streamWriter;
        private IObservable<EventSource<OrderBook>> _observable;

        public StreamEventCommunicationHandler(IMapper mapper,
                                    IDataStreamSource streamSource,
                                    ILogger<StreamEventCommunicationHandler> logger,
                                    StreamEventGrpc.StreamEventGrpcClient client)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _observable = _streamSource.OrderBookStream.DistinctUntilChanged().Do(SendEvent);
            var call = _client.RunStreamEvent(cancellationToken: stoppingToken);
            _streamWriter = call.RequestStream;
            _observable.Subscribe(SendEvent, HandleError, HandleCompletion, stoppingToken);
            await stoppingToken.WaitUntilCancelled();
        }

        /// <summary>
        /// Handler responsable to execute the strategy logic.
        /// </summary>
        public async void SendEvent(EventSource<OrderBook> eventSource)
        {
            try
            {
                var eventSourceDTO = _mapper.Map<EventSourceDTO>(eventSource);
                await _streamWriter.WriteAsync(eventSourceDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Handler responsable to execute the logic in case of an error in subscription.
        /// </summary>
        public void HandleError(Exception exception)
        {
            _logger.LogError(exception.Message);
        }

        /// <summary>
        /// Handler responsable to execute the logic in case of an completion in subscription.
        /// </summary>
        public void HandleCompletion()
        {
            _logger.LogInformation("### Communication completed ### ");
        }
    }
}
