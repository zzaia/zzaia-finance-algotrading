using System;
using System.Collections.Generic;
using System.Text;

using MagoTrader.Core.Models;
using MagoTrader.Core.Exchange;

namespace MagoTrader.Exchange
{
    public class ApiOptions
    {
        public ExchangeName? Name { get; set; }
        public object Authentication { get; set; }
        public ApiOptions()
        {

        }

        public Uri PublicBaseAddress { get; set; }
        public Uri PrivateBaseAddress { get; set; }
    }
}
