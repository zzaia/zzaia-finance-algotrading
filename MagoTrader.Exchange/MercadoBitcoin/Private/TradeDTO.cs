using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    class TradesDTO
    {
        public TradeDTO[] Trades { get; set; }
        public string error { get; set; }
    }
    class TradeDTO
    {
        public int date { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public int tid { get; set; }
        public string type { get; set; }
    }
}
