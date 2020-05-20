using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    class OHLCVDTO
    {
        public string date { get; set; }
        public decimal opening { get; set; }
        public decimal closing { get; set; }
        public decimal lowest { get; set; }
        public decimal highest { get; set; }
        public decimal volume { get; set; }
        public decimal quantity { get; set; }
        public int amount { get; set; }
        public decimal avg_price { get; set; }
        public string error { get; set; }

    }
}
