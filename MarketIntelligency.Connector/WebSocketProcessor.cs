﻿using Crypto.Websocket.Extensions.Core.Models;
using Crypto.Websocket.Extensions.Core.OrderBooks.Models;
using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.EventManager;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Connector
{
    public class WebSocketProcessor : BackgroundService
    {
        private readonly WebSocketConnectorOptions _options;
        private readonly IExchangeSelector _exchangeSelector;
        private readonly IDataStreamSource _dataStreamSource;
        private readonly ILogger<WebSocketProcessor> _logger;
        private readonly TelemetryClient _telemetryClient;

        /// <summary>
        /// Provide data streams from web socket clients
        /// </summary>
        public WebSocketProcessor(Action<WebSocketConnectorOptions> connectorOptions,
            IExchangeSelector exchangeSelector,
            IDataStreamSource dataStreamSource,
            ILogger<WebSocketProcessor> logger,
            TelemetryClient telemetryClient)
        {
            connectorOptions = connectorOptions ?? throw new ArgumentNullException(nameof(connectorOptions));
            var connectorOptionsModel = new WebSocketConnectorOptions();
            connectorOptions.Invoke(connectorOptionsModel);
            _options = connectorOptionsModel;
            _exchangeSelector = exchangeSelector ?? throw new ArgumentNullException(nameof(exchangeSelector));
            _dataStreamSource = dataStreamSource ?? throw new ArgumentNullException(nameof(dataStreamSource));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var exchange = _exchangeSelector.SelectByName(_options.ExchangeName);
            if (exchange.Info.Options.HasWebSocket)
            {
                foreach (var item in _options.DataIn)
                {
                    if (item.GetType() == typeof(Market))
                    {
                        if (_options.DataOut.Any(each => each.Equals(typeof(OrderBook))))
                        {
                            exchange.SetOrderBookSubscription(item);
                        }
                    }
                }

                exchange.SubscribeToOrderBook(ConvertToOrderBook);
                return Task.CompletedTask;
            }
            else
            {
                throw new ArgumentException("Exchange does not support web socket.", nameof(ExchangeName));
            }
        }

        private void ConvertToOrderBook(IList<IOrderBookChangeInfo> orderbookChangeCollection)
        {
            foreach (var orderbookChange in orderbookChangeCollection)
            {
                var orderBookToReturn = new OrderBook()
                {

                    //Exchange = Enumeration.FromDisplayName<ExchangeName>(orderbookChange.ExchangeName),
                    Asks = orderbookChange.Levels.Where(each => each.Side == CryptoOrderSide.Ask)
                                                 .Select(each => new Tuple<decimal, decimal>((decimal)each.Price, (decimal)each.Amount)),
                    Bids = orderbookChange.Levels.Where(each => each.Side == CryptoOrderSide.Bid)
                                                 .Select(each => new Tuple<decimal, decimal>((decimal)each.Price, (decimal)each.Amount))
                };
                PublishEvent(orderBookToReturn);
            }
        }

        private void PublishEvent<T>(T content) where T : class
        {
            var eventToPublish = new EventSource<T>(content);
            _dataStreamSource.Publish(eventToPublish, _logger);
        }
    }
}
