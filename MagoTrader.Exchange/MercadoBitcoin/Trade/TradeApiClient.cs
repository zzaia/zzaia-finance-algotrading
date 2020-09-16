﻿using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;
using MagoTrader.Exchange.MercadoBitcoin.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MagoTrader.Exchange.MercadoBitcoin.Trade
{
    public class TradeApiClient : ApiClientBase, ITradeApiClient
    {
        private readonly HttpClient _client;
        private readonly bool _continueOnCapturedContext;
        private readonly string _requestPath;

        public TradeApiClient(HttpClient client, ILogger<TradeApiClient> logger)
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

        public async Task<Response<TAPResponse<OrderDTO>>> PlaceMarketBuyOrderAsync(ClientCredential clientCredential, string tickerPair, string cost)
        {
            _logger.LogInformation($"Place market buy order {tickerPair}.");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "place_market_buy_order"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("coin_pair", tickerPair),
                    new KeyValuePair<string, string>("cost", cost),
                };
            return await PostSuppreme<OrderDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<OrderDTO>>> PlaceMarketSellOrderAsync(ClientCredential clientCredential, string tickerPair, string quantity)
        {
            _logger.LogInformation($"Place market sell order {tickerPair}.");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "place_market_sell_order"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("coin_pair", tickerPair),
                    new KeyValuePair<string, string>("quantity", quantity),
                };
            return await PostSuppreme<OrderDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<OrderDTO>>> CancelOrderAsync(ClientCredential clientCredential, string tickerPair, int orderId)
        {
            _logger.LogInformation($"Cancel order by {tickerPair} and id.");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "cancel_order"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("coin_pair", tickerPair),
                    new KeyValuePair<string, string>("order_id", orderId.ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<OrderDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<WithdrawalDTO>>> GetWithdrawalAsync(ClientCredential clientCredential, string ticker, int withdrawalId)
        {
            _logger.LogInformation($"Get {ticker} withdrawal.");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "get_withdrawal"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("coin", ticker),
                    new KeyValuePair<string, string>("withdrawal_id", withdrawalId.ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<WithdrawalDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<WithdrawalDTO>>> PlaceWithdrawalAsync(ClientCredential clientCredential, string ticker, string walletAddress, 
                                                                                                                                       string quantity, 
                                                                                                                                       string transactionFee,
                                                                                                                                       int destinationTag = int.MinValue,
                                                                                                                                       bool isAggregate = false,
                                                                                                                                       bool inBlockchain = false)
        {
            _logger.LogInformation($"Place withdrawal {ticker}");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "withdraw_coin"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("coin", ticker),
                    new KeyValuePair<string, string>("address", walletAddress),
                    new KeyValuePair<string, string>("quantity", quantity),
                    new KeyValuePair<string, string>("tx_fee", transactionFee),
                    new KeyValuePair<string, string>("destination_tag", (destinationTag.Equals(int.MinValue) ? String.Empty : destinationTag.ToString(CultureInfo.InvariantCulture))),
                    new KeyValuePair<string, string>("tx_aggregate", isAggregate.ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("via_blockchain", inBlockchain.ToString(CultureInfo.InvariantCulture)),
                };
            return await PostSuppreme<WithdrawalDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TAPResponse<WithdrawalDTO>>> PlaceWithdrawalAsync(ClientCredential clientCredential, string accountRef, string quantity)
        {
            _logger.LogInformation($"Place BRL withdrawal");
            var parameters = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("tapi_method", "withdraw_coin"),
                    new KeyValuePair<string, string>("tapi_nonce", DateTimeUtils.CurrentUtcTimestamp().ToString(CultureInfo.InvariantCulture)),
                    new KeyValuePair<string, string>("coin", "BRL"),
                    new KeyValuePair<string, string>("account_ref", accountRef),
                    new KeyValuePair<string, string>("quantity", quantity),
                };
            return await PostSuppreme<WithdrawalDTO>(clientCredential, parameters).ConfigureAwait(_continueOnCapturedContext);
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
