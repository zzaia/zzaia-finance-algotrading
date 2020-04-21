using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Core.Exchange
{

    public class MarketFee
    {
        public bool TierBasedPercentage { get; set; }
        public bool FixedPercentage { get; set; }
        public decimal Maker { get; set; }
        public decimal Taker { get; set; }
    }

}
