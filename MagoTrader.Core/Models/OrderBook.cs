using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Core.Models
{
    public class OrderBook
    {
        public Decimal[][] Bids { get; set; }
        public Decimal[][] Asks { get; set; }
      
    }
}
