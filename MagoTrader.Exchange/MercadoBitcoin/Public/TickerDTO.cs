using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    class TickerDTO
    {
        public string high { get; set; }
        public string low { get; set; }
        public string vol { get; set; }
        public string last { get; set; }
        public string buy { get; set; }
        public string sell { get; set; }
        public int date { get; set; }

    }

    class TickerDataDTO
    {
        public TickerDTO ticker { get; set; }
        public string error { get; set; }

    }
}
