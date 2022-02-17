using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.Web.Grpc.Protos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Web.Grpc
{
    public class CommunicationHandler : BackgroundService
    {
        private IObservable<OrderBook> _observable;
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<CommunicationHandler> _logger;
        private readonly StreamEventGrpc.StreamEventGrpcClient _client;
        private IClientStreamWriter<EventSourceDTO> _streamWriter;

        public CommunicationHandler(IDataStreamSource streamSource,
                                    ILogger<CommunicationHandler> logger,
                                    StreamEventGrpc.StreamEventGrpcClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _observable = _streamSource.OrderBookSnapshotStream
                                       .Select(each => each.Content)
                                       .DistinctUntilChanged();
            var call = _client.RunStreamEvent(cancellationToken: stoppingToken);
            _streamWriter = call.RequestStream;
            _observable.Subscribe(SendEvent, HandleError, HandleCompletion, stoppingToken);
            await stoppingToken.WaitUntilCancelled();
        }

        /// <summary>
        /// Handler responsable to execute the strategy logic.
        /// </summary>
        public async void SendEvent(OrderBook orderBook)
        {
            _logger.LogInformation("### Sending event for communication ###");
            try
            {
                var orderBookDTO = new OrderbookDTO()
                {
                    ExchangeName = orderBook.Exchange.DisplayName,
                    //Market = orderBook.Market.Ticker
                };
                var eventSource = new EventSourceDTO() { Content = Any.Pack(orderBookDTO), Type = nameof(OrderBook) };
                await _streamWriter.WriteAsync(eventSource);
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
