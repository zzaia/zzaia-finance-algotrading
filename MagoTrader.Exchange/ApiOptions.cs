using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange
{
    public class ApiOptions
    {
        public ApiOptions(string exchange)
        {
            switch(exchange)
            {
                case "MercadoBitcoin":
                    PublicBaseAddress = new Uri("https://www.mercadobitcoin.net/api");
                    PrivateBaseAddress = new Uri("https://www.mercadobitcoin.net/tapi/v3");
                    break;

            }
        }

        public Uri PublicBaseAddress { get; set; }
        public Uri PrivateBaseAddress { get; set; }
    }
}
