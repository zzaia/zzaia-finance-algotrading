using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    class OrderbookDTO
    {
        public Decimal[][] asks { get; set; }
        public Decimal[][] bids { get; set; }
        public string ticker { get; set; }
        public string error { get; set; }

    }
}
