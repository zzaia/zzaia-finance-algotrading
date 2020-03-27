using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    class TradeDTO
    {
        public string dates { get; set; }
        public Decimal price { get; set; }
        public double amount { get; set; }
        public int tid { get; set; }
        public string type { get; set; }
    }
}
