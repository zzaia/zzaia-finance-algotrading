﻿using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderAgregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Application.Strategies.MarketMaker

{
    public class MarketMakerHandler : BackgroundService
    {
        private readonly IDataStreamSource _streamSource;
        private readonly ILogger<MarketMakerHandler> _logger;
        private IObservable<OrderBook> _observable;

        public MarketMakerHandler(IDataStreamSource streamSource, ILogger<MarketMakerHandler> logger)
        {
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _observable = _streamSource.OrderBookSnapshotStream
                                       .Select(each => each.Content)
                                       .DistinctUntilChanged();
            _observable.Subscribe(HandleStrategy, HandleError, HandleCompletion, stoppingToken);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Handler responsable to execute the strategy logic.
        /// </summary>
        public async void HandleStrategy(OrderBook orderBook)
        {
            _logger.LogInformation("### Consuming event for market making strategy ###");
            await Task.Delay(1000);
            var response = new EventSource<Order>(new Order());
            _streamSource.Publish(response);
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
            _logger.LogInformation("### Market Making Strategy completed ### ");
        }
    }
}