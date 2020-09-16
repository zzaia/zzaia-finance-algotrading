using System;
using System.Collections.Generic;
using System.Text;

namespace MarketMaker.Core.Exchange
{
    public class ExchangeUris
    {
        public Uri Logo { get; set; }
        public Uri WWW { get; set; }
        public IEnumerable<Uri> Fees { get; set; }
        public bool Testnet { get; set; }
        public ApiUris Api { get; set; }
        public IEnumerable<Uri> Doc { get; set; }
    }
    public class ApiUris
    {
        public Uri Public { get; set; }
        public Uri Private { get; set; }
        public Uri Trade { get; set; }
    }
}
