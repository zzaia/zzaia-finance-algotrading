using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    class OHLCVDTO
    {
        public string date { get; set; }
        public Decimal opening { get; set; }
        public Decimal closing { get; set; }
        public Decimal lowest { get; set; }
        public Decimal highest { get; set; }
        public Decimal volume { get; set; }
        public Decimal quantity { get; set; }
        public Decimal amount { get; set; }
        public Decimal avg_price { get; set; }
        public string error { get; set; }

    }
}
