using MarketMaker.Core.Exchange;
using System;
using System.Collections.Generic;

namespace MarketMaker.Core.Models
{
    /// <summary>
    /// Represents a financial market candlestick.
    /// </summary>
    public class OHLCV
    {
        /// <summary>
        /// Represents exchange where the data was taken.
        /// </summary>
        public ExchangeNameEnum Exchange { get; set; }

        /// <summary>
        /// Represents the data market, with main and base assets.
        /// </summary>
        public Market Market { get; set; }

        /// <summary>
        /// Represents the candlestick time of measurement.
        /// </summary>
        public DateTimeOffset DateTimeOffset { get; set; }

        /// <summary>
        /// Represents the candlestick timeframe.
        /// </summary>
        public TimeFrame TimeFrame { get; set; }
        
        /// <summary>
        /// Opening main asset price for timeframe candlestick.
        /// </summary>
        public decimal Open { get; set; }
        
        /// <summary>
        /// Highest main asset price for timeframe candlestick.
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Lowest main asset price for timeframe candlestick.
        /// </summary>
        public decimal Low { get; set; }

        /// <summary>
        /// Closing main asset price for timeframe candlestick.
        /// </summary>
        public decimal Close { get; set; }

        /// <summary>
        /// Last main asset price, without closing the timeframe candlestick.
        /// </summary>
        public decimal Last { get; set; }

        /// <summary>
        /// Biggest asset price for selling in the timeframe candlestick.
        /// </summary>
        public decimal Sell { get; set; }

        /// <summary>
        /// Biggest asset price for buying in the timeframe candlestick.
        /// </summary>
        public decimal Buy { get; set; }

        /// <summary>
        /// Volume of value, in market base asset, traded in the timeframe.
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Total quantity of main asset traded in the timeframe.
        /// </summary>
        public decimal TradedQuantity { get; set; }

        /// <summary>
        /// Average price of main asset traded in the timeframe.
        /// </summary>
        public decimal Average { get; set; }

        /// <summary>
        /// Total number of trades in the timeframe.
        /// </summary>
        public int NumberOfTrades { get; set; }

    }
}
