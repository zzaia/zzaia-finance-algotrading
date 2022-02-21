using Crypto.Websocket.Extensions.Core.Models;
using Crypto.Websocket.Extensions.Core.OrderBooks;
using Crypto.Websocket.Extensions.Core.OrderBooks.Models;
using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
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
using OrderBookLevelModel = MarketIntelligency.Core.Models.OrderBookAggregate.OrderBookLevel;

namespace MarketIntelligency.Connector
{
    public class WebSocketProcessor : BackgroundService
    {
        private readonly WebSocketConnectorOptions _options;
        private readonly IExchangeSelector _exchangeSelector;
        private readonly IDataStreamSource _dataStreamSource;
        private readonly ILogger<WebSocketProcessor> _logger;
        private readonly TelemetryClient _telemetryClient;
        public IOrderBookChangeInfo SnapShot { get; set; }

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
            if (!_options.DataIn.Any()) new ArgumentException("No subscriptions data in type provided", nameof(_options.DataIn));
            if (!_options.DataOut.Any()) new ArgumentException("No subscriptions data out type provided", nameof(_options.DataOut));
            var exchange = _exchangeSelector.SelectByName(_options.ExchangeName);
            if (exchange.Info.Options.HasWebSocket)
            {
                foreach (var item in _options.DataIn)
                {
                    if (item.GetType() == typeof(Market))
                    {
                        if (_options.DataOut.Any(each => each.Name.Equals(nameof(OrderBook))))
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
                PublishEvent(orderbookChange);

                //try
                //{
                //    var asks = new List<OrderBookLevelModel>();
                //    var bids = new List<OrderBookLevelModel>();
                //    if (orderbookChange.IsSnapshot)
                //    {
                //        SnapShot = orderbookChange;
                //        asks = SnapShot.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                               .Where(each => each.Action == OrderBookAction.Insert)
                //                               .Single()
                //                               .Levels
                //                               .Where(each => each.Side == CryptoOrderSide.Ask)
                //                               .Select(each => new OrderBookLevelModel(each.Id, (decimal)each.Price, (decimal)each.Amount))
                //                               .ToList();
                //        bids = SnapShot.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                               .Where(each => each.Action == OrderBookAction.Insert)
                //                               .Single()
                //                               .Levels
                //                               .Where(each => each.Side == CryptoOrderSide.Bid)
                //                               .Select(each => new OrderBookLevelModel(each.Id, (decimal)each.Price, (decimal)each.Amount))
                //                               .ToList();
                //        var orderBookToReturn = new OrderBook()
                //        {
                //            DateTimeOffset = orderbookChange.ServerTimestamp ?? DateTimeOffset.UtcNow,
                //            Market = new Market(orderbookChange.Pair),
                //            Exchange = Enumeration.FromDisplayName<ExchangeName>(orderbookChange.ExchangeName),
                //            Asks = asks,
                //            Bids = bids,
                //        };
                //        PublishEvent(orderBookToReturn);
                //    }
                //    else if (SnapShot != null)
                //    {
                //        var asksDiff = orderbookChange.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                                      .Where(each => each.Action == OrderBookAction.Update)
                //                                      .Single()
                //                                      .Levels
                //                                      .Where(each => each.Side == CryptoOrderSide.Ask)
                //                                      .Select(each => new Tuple<string, double?, double>(each.Id, each.Price, each.AmountDifference));
                //        var asksLevels = SnapShot.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                                          .Where(each => each.Action == OrderBookAction.Insert)
                //                                          .Single()
                //                                          .Levels
                //                                          .Where(each => each.Side == CryptoOrderSide.Ask);
                //        foreach (var diff in asksDiff)
                //        {
                //            var amount = asksLevels.Single(one => one.Id == diff.Item1).Amount + diff.Item2;
                //            asks.Add(new OrderBookLevelModel(diff.Item1, (decimal)diff.Item2, (decimal)amount));
                //        }
                //        var bidsDiff = orderbookChange.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                                      .Where(each => each.Action == OrderBookAction.Update)
                //                                      .Single()
                //                                      .Levels
                //                                      .Where(each => each.Side == CryptoOrderSide.Bid)
                //                                      .Select(each => new Tuple<string, double?, double>(each.Id, each.Price, each.AmountDifference));
                //        var bidsLevels = SnapShot.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                                          .Where(each => each.Action == OrderBookAction.Insert)
                //                                          .Single()
                //                                          .Levels
                //                                          .Where(each => each.Side == CryptoOrderSide.Bid);
                //        foreach (var diff in bidsDiff)
                //        {
                //            var amount = asksLevels.Single(one => one.Id == diff.Item1).Amount + diff.Item2;
                //            asks.Add(new OrderBookLevelModel(diff.Item1, (decimal)diff.Item2, (decimal)amount));
                //        }

                //        var asksRemoval = orderbookChange.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                                         .Where(each => each.Action == OrderBookAction.Delete)
                //                                         .Single()
                //                                         .Levels
                //                                         .Where(each => each.Side == CryptoOrderSide.Ask)
                //                                         .Select(each => new Tuple<string, double?, double>(each.Id, each.Price, each.AmountDifference));
                //        foreach (var itemToRemove in asksRemoval)
                //        {
                //            asks.Remove(new OrderBookLevelModel(itemToRemove.Item1, (decimal)itemToRemove.Item2, (decimal)itemToRemove.Item3));
                //        }

                //        var bidsRemoval = orderbookChange.Sources.Where(each => each.OrderBookType == CryptoOrderBookType.L2)
                //                                         .Where(each => each.Action == OrderBookAction.Delete)
                //                                         .Single()
                //                                         .Levels
                //                                         .Where(each => each.Side == CryptoOrderSide.Bid)
                //                                         .Select(each => new Tuple<string, double?, double>(each.Id, each.Price, each.AmountDifference));
                //        foreach (var itemToRemove in asksRemoval)
                //        {
                //            bids.Remove(new OrderBookLevelModel(itemToRemove.Item1, (decimal)itemToRemove.Item2, (decimal)itemToRemove.Item3));
                //        }

                //        var orderBookToReturn = new OrderBook()
                //        {
                //            DateTimeOffset = orderbookChange.ServerTimestamp ?? DateTimeOffset.UtcNow,
                //            Market = new Market(orderbookChange.Pair),
                //            Exchange = Enumeration.FromDisplayName<ExchangeName>(orderbookChange.ExchangeName),
                //            Asks = asks,
                //            Bids = bids,
                //        };
                //        PublishEvent(orderBookToReturn);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    continue;
                //}
            }
        }

        private void PublishEvent<T>(T content) where T : class
        {
            var eventToPublish = new EventSource<T>(content);
            _dataStreamSource.Publish(eventToPublish, _logger);
        }
    }
}
