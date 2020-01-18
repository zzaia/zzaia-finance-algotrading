using System;
using System.Collections.Generic;

namespace MagoTrader.Core.Models
{
    public class OHLCV
    {
        public Exchange Exchange { get; set; }
        public Ticker Ticker { get; set; }
        public DateTime DateTime { get; set; }
        public Decimal Open { get; set; }
        public Decimal High { get; set; }
        public Decimal Low { get; set; }
        public Decimal Close { get; set; }
        public Decimal Volume { get; set; }
        public List<Price> Bids { get; set; }
        public List<Price> Asks { get; set; }
        public OHLCV()
        {
            Bids = new List<Price>();
            Asks = new List<Price>();
        }

    }
}
