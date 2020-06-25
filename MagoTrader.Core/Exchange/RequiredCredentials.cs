using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Core.Exchange
{
    public class RequiredCredentials
    {
        public bool Id { get; set; }
        public bool Secret { get; set; }
        public bool Login { get; set; }
        public bool Password { get; set; }
        public bool Twofa { get; set; }
    }
}
