using System;
using System.Collections.Generic;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ExchangeUris
    {
        public Uri Logo { get; set; }
        public Uri WWW { get; set; }
        public IEnumerable<Uri> Fees { get; set; }
        public bool Testnet { get; set; }
        public WebApiUris WebApi { get; set; }
        public WebSocketUris WebSocket { get; set; }
        public IEnumerable<Uri> Doc { get; set; }
    }
    public class WebApiUris
    {
        public Uri Public { get; set; }
        public Uri Private { get; set; }
        public Uri Trade { get; set; }
    }

    public class WebSocketUris
    {
        public Uri Main { get; set; }
        public Uri Future { get; set; }
    }
}
