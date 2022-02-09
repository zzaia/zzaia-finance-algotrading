﻿using Dapr.Client;
using Google.Protobuf.WellKnownTypes;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Application.Adapter.Binance
{
    public class CommunicationHandler : BackgroundService
    {
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<CommunicationHandler> _logger;
        private readonly DaprClient _client;
        private IObservable<OrderBook> _observable;

        public CommunicationHandler(IDataStreamSource streamSource,
                                    ILogger<CommunicationHandler> logger,
                                    DaprClient client)
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
            var orderBookDTO = new OrderBookDTO() { }
            var eventSource = new EventSource<OrderBook>(orderBook);
            //var eventMessage = Any.Pack(eventSource);
            var response = await _client.InvokeMethodGrpcAsync<Any, Any>("data-event-manager", "orderbook", eventSource);
            var input = response.Unpack<Response>();
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
