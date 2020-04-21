using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

using MagoTrader.Core.Exchange;


namespace MagoTrader.Exchange.MercadoBitcoin.Private
{
    public class PrivateApiClient : IPrivateApiClient
    {
        protected readonly AuthApiOptions _authentication;
        protected readonly ILogger<PrivateApiClient> _logger;
        protected HttpClient _client;

        public PrivateApiClient(HttpClient client, IOptionsMonitor<AuthApiOptions> authApiOptions, ILogger<PrivateApiClient> logger) 
        {
            _authentication = authApiOptions.Get("MercadoBitcoinPrivateApiOptions") ?? throw new ArgumentNullException(nameof(authApiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

    }
}
