using MagoTrader.Core.Exchange;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace MagoTrader.Exchange.MercadoBitcoin.Trade
{
    public class TradeApiClient : ApiClientBase, ITradeApiClient
    {
        private readonly HttpClient _client;

        public TradeApiClient(HttpClient client, ILogger<TradeApiClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }
    }
}
