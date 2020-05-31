using MagoTrader.Core.Exchange;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin.Trade
{
    public class TradeApiClient : ITradeApiClient
    {
        //private readonly AuthApiOptions _authentication;
        private readonly ILogger<TradeApiClient> _logger;
        private readonly HttpClient _client;

        public TradeApiClient(HttpClient client, ILogger<TradeApiClient> logger)//, IOptionsMonitor<AuthApiOptions> authApiOptions)
        {
            //authApiOptions = authApiOptions ?? throw new ArgumentNullException(nameof(authApiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            //_authentication = authApiOptions.Get("MercadoBitcoinTradeApiOptions") ?? throw new ArgumentNullException(nameof(authApiOptions));
        }
        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }
    }
}
