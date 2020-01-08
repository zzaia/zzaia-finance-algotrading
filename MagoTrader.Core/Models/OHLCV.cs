﻿using System;
using System.Collections.Generic;

namespace MagoTrader.Core.Models
{
    public class OHLCV
    {
        public double Open {get; set;}
        public double High {get; set;}
        public double Low {get; set;}
        public double Close {get; set;}
        public double Volume {get; set;}
        public List<double> Bids {get; set;}
        public List<double> Asks {get; set;}
        public DateTime DateTime {get; set;}
    }
}
