using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;

namespace MarketIntelligency.Core.Models.OrderBookAggregate
{
    /// <summary>
    /// Order book for market main asset, from OHLCV date time.
    /// </summary>
    public class OrderBook
    {
        public OrderBook() { }
        /// <summary>
        /// Represents exchange where the data was taken.
        /// </summary>
        public ExchangeName Exchange { get; set; }

        /// <summary>
        /// Represents the data market, with main and base assets.
        /// </summary>
        public Market Market { get; set; }

        /// <summary>
        /// Represents the candlestick time of measurement.
        /// </summary>
        public DateTimeOffset ServerTimeStamp { get; set; }

        /// <summary>
        /// List of bids orders, price and volume.
        /// </summary>
        public IEnumerable<OrderBookLevel> Bids { get; set; }

        /// <summary>
        /// List of asks orders, price and volume.
        /// </summary>
        public IEnumerable<OrderBookLevel> Asks { get; set; }

    }
}
