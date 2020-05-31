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
        //private readonly AuthApiOptions _authentication;
        private readonly ILogger<PrivateApiClient> _logger;
        private readonly HttpClient _client;

        public PrivateApiClient(HttpClient client, ILogger<PrivateApiClient> logger)//, IOptionsMonitor<AuthApiOptions> authApiOptions) 
        {
            //_authentication = authApiOptions.Get("MercadoBitcoinPrivateApiOptions") ?? throw new ArgumentNullException(nameof(authApiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_authentication = authApiOptions.Get("MercadoBitcoinPrivateApiOptions") ?? throw new ArgumentNullException(nameof(authApiOptions));
        }
        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }


    }
}
