using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    class OrderbookDTO
    {
        public Decimal[][] asks { get; set; }
        public Decimal[][] bids { get; set; }
        public string ticker { get; set; }
    }
}
