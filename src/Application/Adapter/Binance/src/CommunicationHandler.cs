using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderAgregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using MarketIntelligency.WebGrpc.Protos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using MarketIntelligency.Web.Grpc.Clients;

namespace MarketIntelligency.Application.Adapter.Binance
{
    public class CommunicationHandler : BackgroundService
    {
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<CommunicationHandler> _logger;
        private readonly StreamEventGrpc.StreamEventGrpcClient _client;
        private IObservable<OrderBook> _observable;

        public CommunicationHandler(IDataStreamSource streamSource,
                                    ILogger<CommunicationHandler> logger,
                                    StreamEventGrpc.StreamEventGrpcClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _observable = _streamSource.OrderBookSnapshotStream
                                       .Select(each => each.Content)
                                       .DistinctUntilChanged();
            _observable.Subscribe(SendEvent, HandleError, HandleCompletion, stoppingToken);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Handler responsable to execute the strategy logic.
        /// </summary>
        /// 
        public async void SendEvent(OrderBook orderBook)
        {
            _logger.LogInformation("### Consuming event for communication ###");
            var eventMessage = new EventMessage() { Content = Any.Pack(orderBook) };
            await _client.ReceiveEvent(eventMessage);
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
