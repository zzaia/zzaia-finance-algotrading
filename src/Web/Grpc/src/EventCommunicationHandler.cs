using AutoMapper;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.Web.Grpc.Protos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc
{
    public class EventCommunicationHandler : BackgroundService
    {
        private readonly IMapper _mapper;
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<EventCommunicationHandler> _logger;
        private readonly EventGrpc.EventGrpcClient _client;
        private IObservable<EventSource<OrderBook>> _observable;

        public EventCommunicationHandler(IMapper mapper,
                                    IDataStreamSource streamSource,
                                    ILogger<EventCommunicationHandler> logger,
                                    EventGrpc.EventGrpcClient client)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _observable = _streamSource.OrderBookStream.DistinctUntilChanged();
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
                await _client.RunEventAsync(eventSourceDTO);
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
