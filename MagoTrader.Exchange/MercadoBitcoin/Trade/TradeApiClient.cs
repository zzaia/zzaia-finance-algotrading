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
        protected readonly AuthApiOptions _authentication;
        protected readonly ILogger<TradeApiClient> _logger;
        protected HttpClient _client;

        public TradeApiClient(HttpClient client, IOptionsMonitor<AuthApiOptions> authApiOptions, ILogger<TradeApiClient> logger)
        {
            _authentication = authApiOptions.Get("MercadoBitcoinTradeApiOptions") ?? throw new ArgumentNullException(nameof(authApiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
