using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using MagoTrader.Core.Exchange;


namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class PrivateApiClient : PublicApiClient , IPrivateApiClient
    {
        private object _authentication;
        public PrivateApiClient(HttpClient client, IOptionsMonitor<ApiOptions> apiOptions, ILogger<PublicApiClient> logger, object authentication) : base(client, apiOptions, logger)
        {
            _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
        }

    }
}
