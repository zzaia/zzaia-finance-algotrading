using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Utils;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Models.DTO;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Private;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Private
{
    public class PrivateApiClient : ApiClientBase, IPrivateApiClient
    {
        private readonly HttpClient _client;
        private readonly bool _continueOnCapturedContext;
        private readonly string _requestPath;

        public PrivateApiClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _requestPath = "/tapi/v3/";
            _continueOnCapturedContext = false;
        }

        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }

        public async Task<Response<TAPResponse<SystemMessagesDTO>>> GetListOfSystemMessagesAsync(ClientCredential clientCredential, string level, CancellationToken cancellationToken = new CancellationToken())
        {
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "list_system_messages"),
                    new KeyValuePair<string, string>("level", level),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<SystemMessagesDTO>(clientCredential, parameters, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<AccountInformationDTO>>> GetAccountInformationAsync(ClientCredential clientCredential, CancellationToken cancellationToken = new CancellationToken())
        {
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "get_account_info"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<AccountInformationDTO>(clientCredential, parameters, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<OrderInformationDTO>>> GetOrderByIdAsync(ClientCredential clientCredential, int orderId, string tickerPair, CancellationToken cancellationToken = new CancellationToken())
        {
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "get_order"),
                    new KeyValuePair<string, string>("coin_pair", tickerPair),
                    new KeyValuePair<string, string>("order_id", orderId.ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<OrderInformationDTO>(clientCredential, parameters, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<OrdersInformationDTO>>> GetListOfOrdersAsync(ClientCredential clientCredential, string tickerPair,
                                                                                                                               string statusList,
                                                                                                                               bool hasFills, CancellationToken cancellationToken)
        {
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "list_orders"),
                    new KeyValuePair<string, string>("coin_pair", tickerPair),
                    new KeyValuePair<string, string>("status_list", statusList),
                    new KeyValuePair<string, string>("has_fills", hasFills.ToString()),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<OrdersInformationDTO>(clientCredential, parameters, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<OrderbookInformationDTO>>> GetCompleteOrderBookByTickerPairAsync(ClientCredential clientCredential, string tickerPair, bool fullQuantity, CancellationToken cancellationToken = new CancellationToken())
        {
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "list_orderbook"),
                    new KeyValuePair<string, string>("coin_pair", tickerPair),
                    new KeyValuePair<string, string>("full", fullQuantity.ToString()),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<OrderbookInformationDTO>(clientCredential, parameters, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
        }

        private async Task<Response<TAPResponse<T>>> PostSuppreme<T>(ClientCredential clientCredential, IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken)
        {
            using var requestBody = new FormUrlEncodedContent(parameters);
            string paramString = await requestBody.ReadAsStringAsync(cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            string requestBodyParams = $"{_requestPath}?{paramString}";
            string hmac = AuthenticationUtils.SignMessage(clientCredential.Secret, requestBodyParams);
            Uri requestUri = new(_requestPath, UriKind.Relative);
            using var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = requestBody };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            request.Content.Headers.Add("TAPI-ID", clientCredential.Id);
            request.Content.Headers.Add("TAPI-MAC", hmac);
            var response = await _client.SendAsync(request, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<TAPResponse<T>>(response).ConfigureAwait(_continueOnCapturedContext);
        }
    }
}