using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx.WebSocket.Models
{
    public class OrderbookResponse
    {

        public OrderbookData Data { get; set; }
    }
    public class OrderbookData
    {
        public string Action { get; set; }
    }
}
