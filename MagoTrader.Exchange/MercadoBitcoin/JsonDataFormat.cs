using System;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class JsonDataFormat
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

        public Decimal[][] asks { get; set; }
        public Decimal[][] bids { get; set; }
        public ticker ticker  { get; set; }
    }

    public class ticker
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