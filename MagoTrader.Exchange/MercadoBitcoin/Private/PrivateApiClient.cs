using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;
using MagoTrader.Exchange.MercadoBitcoin.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagoTrader.Exchange.MercadoBitcoin.Private
{
    public class PrivateApiClient : ApiClientBase, IPrivateApiClient
    {
        private readonly HttpClient _client;
        private readonly bool _continueOnCapturedContext;
        private readonly string _requestPath;

        public PrivateApiClient(HttpClient client, ILogger<PrivateApiClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _requestPath = "/tapi/v3/";
            _continueOnCapturedContext = false;
        }

        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }

        public async Task<Response<TAPResponse<SystemMessagesDTO>>> PostSystemMessagesAsync(ClientCredential clientCredential, SystemMessageTypeEnum level)
        {
            _logger.LogInformation($"Get all system messages.");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "list_system_messages"),
                    new KeyValuePair<string, string>("level", level.ToString()),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<SystemMessagesDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<T>>> PostSuppreme<T>(ClientCredential clientCredential, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            using var requestBody = new FormUrlEncodedContent(parameters);
            string paramString = await requestBody.ReadAsStringAsync().ConfigureAwait(_continueOnCapturedContext);
            string requestBodyParams = $"{_requestPath}?{paramString}";
            string hmac = CryptographyUtils.SignMessage(clientCredential.Secret, requestBodyParams);
            Uri requestUri = new Uri(_requestPath, UriKind.Relative);
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = requestBody };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content.Headers.Add("TAPI-ID", clientCredential.Id);
            request.Content.Headers.Add("TAPI-MAC", hmac);
            var response = await _client.SendAsync(request).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<TAPResponse<T>>(response).ConfigureAwait(_continueOnCapturedContext);
        }
    }
}
