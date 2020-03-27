using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    class TickerDTO
    {
        public Decimal high { get; set; }
        public Decimal low { get; set; }
        public Decimal vol { get; set; }
        public Decimal last { get; set; }
        public Decimal buy { get; set; }
        public Decimal sell { get; set; }
        public string date { get; set; }
    }
}
