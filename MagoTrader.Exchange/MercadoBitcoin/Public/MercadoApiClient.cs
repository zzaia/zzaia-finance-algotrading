using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

using System.Text;
using Microsoft.Extensions.Options;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    class MercadoApiClient
    {
        private readonly ApiOptions _apiOptions;
        private readonly ILogger<MercadoApiClient> _logger;

        private HttpClient _client;

        public MercadoApiClient( HttpClient client, IOptionsMonitor<ApiOptions> apiOptions, ILogger<MercadoApiClient> logger)
        {
            _apiOptions = apiOptions?.Get("KycApi") ?? throw new ArgumentNullException(nameof(apiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _client.BaseAddress = _apiOptions.PublicBaseAddress;
        }
    }
}
