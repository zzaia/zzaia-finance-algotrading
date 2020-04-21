using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Core.Exchange
{
    public class ExchangeUrls
    {
        public string Logo { get; set; }
        public string WWW { get; set; }
        public IEnumerable<string> Fees { get; set; }
        public bool Testnet { get; set; }
        public ApiUrls Api { get; set; }
        public IEnumerable<string> Doc { get; set; }
    }
    public class ApiUrls
    {
        public string Public { get; set; }
        public string Private { get; set; }
        public string Trade { get; set; }
    }
}
