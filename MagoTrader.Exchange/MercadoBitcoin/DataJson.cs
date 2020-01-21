using System;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class DataJson
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
    }
}