using System;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class ohlcv
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

    public class orderbook
    { 
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

    public class trade
    {
        public string dates { get; set; }
        public Decimal price { get; set; }
        public double amount { get; set; }
        public int tid { get; set; }
        public string type { get; set; }
    }
}