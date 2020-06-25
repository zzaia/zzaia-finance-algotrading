using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;

namespace MagoTrader.Exchange.MercadoBitcoin.Private
{
    public class PrivateApiClient : ApiClientBase, IPrivateApiClient
    {
        private readonly ILogger<PrivateApiClient> _logger;
        private readonly HttpClient _client;
        private readonly bool _awaitable;
        private readonly string _requestPath;

        public PrivateApiClient(HttpClient client, ILogger<PrivateApiClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _requestPath = "/tapi/v3/";
            _awaitable = false;
        }

        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }

        public async Task<Response<TAPResponse<IEnumerable<SystemMessageDTO>>>> PostSystemMessagesAsync(ClientCredential clientCredential, string level)
        {
            _logger.LogInformation($"Get all system messages.");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "list_system_messages"),
                    new KeyValuePair<string, string>("level", level),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<IEnumerable<SystemMessageDTO>>(clientCredential, parameters).ConfigureAwait(_awaitable);
        }

        public async Task<Response<TAPResponse<T>>> PostSuppreme<T>(ClientCredential clientCredential, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            using var requestBody = new FormUrlEncodedContent(parameters);
            string requestBodyParams = $"{_requestPath}?{requestBody}";
            string hmac = CryptographyUtils.SignMessage(clientCredential.Secret, requestBodyParams);
            _client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
            _client.DefaultRequestHeaders.Add("TAPI-ID", clientCredential.Id);
            _client.DefaultRequestHeaders.Add("TAPI-MAC", hmac);
            Uri requestUri = new Uri(_requestPath, UriKind.Relative);
            var response = await _client.PostAsync(requestUri, requestBody).ConfigureAwait(_awaitable);
            return await this.GetResponseAsync<TAPResponse<T>>(response).ConfigureAwait(_awaitable);
        }
    }
}
