using MagoTrader.Core.Exchange;
using System;
using System.Collections.Generic;

namespace MagoTrader.Core.Models
{
    public class OHLCV
    {
        public ExchangeNameEnum Exchange { get; set; }
        public AssetTickerEnum Ticker { get; set; }
        public DateTime DateTime { get; set; }
        public Decimal Open { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Close { get; set; }
        public Decimal Volume { get; set; }
        public OrderBook OrderBook { get; set; }

    }
}
