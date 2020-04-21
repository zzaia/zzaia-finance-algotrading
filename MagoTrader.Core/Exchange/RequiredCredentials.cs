using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Core.Exchange
{
    public class RequiredCredentials
    {
        public bool Apikey { get; set; }
        public bool Secret { get; set; }
        public bool Uid { get; set; }
        public bool Login { get; set; }
        public bool Password { get; set; }
        public bool Twofa { get; set; }
    }
}
