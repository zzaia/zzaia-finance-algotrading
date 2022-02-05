using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;

namespace MarketIntelligency.Core.Models.OrderBookAgregate
{
    /// <summary>
    /// Order book for market main asset, from OHLCV date time.
    /// </summary>
    public class OrderBook
    {

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
        public DateTimeOffset DateTimeOffset { get; set; }

        /// <summary>
        /// List of bids orders, price and volume.
        /// </summary>
        public IEnumerable<Tuple<decimal, decimal>> Bids { get; set; }

        /// <summary>
        /// List of asks orders, price and volume.
        /// </summary>
        public IEnumerable<Tuple<decimal, decimal>> Asks { get; set; }

    }
}
