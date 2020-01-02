using System;
using System.Collections.Generic;

namespace MagoTrader.Core
{
    public class OHLCV
    {
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public double Volume;
        public List<double> Bids;
        public List<double> Asks;
        public DateTime DateTime;
    }
}
