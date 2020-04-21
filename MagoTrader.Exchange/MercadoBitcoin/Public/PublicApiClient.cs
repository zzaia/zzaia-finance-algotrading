using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

using System.Text;
using Microsoft.Extensions.Options;

using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;

using System.Text.Json;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    public class PublicApiClient : IPublicApiClient
    {
        protected readonly ILogger<PublicApiClient> _logger;
        protected HttpClient _client;

        private JsonSerializerOptions _jsonOptions;
        public PublicApiClient(HttpClient client, ILogger<PublicApiClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _client.BaseAddress = new Uri("https://www.mercadobitcoin.net/api/");
            _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }
        
        public async Task<OHLCV> GetDaySummaryOHLCVAsync(AssetTickerEnum ticker, DateTime dt)
        {
            Uri requestUri = new Uri($"{ticker.ToString()}/day-summary/{dt.Year}/{dt.Month}/{dt.Day}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri);

            if(response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var OHLCVFromApi = await JsonSerializer.DeserializeAsync<OHLCVDTO>(responseStream, _jsonOptions);
                if(String.IsNullOrEmpty(OHLCVFromApi.error))
                {
                    _logger.LogInformation($"Get {ticker.ToString()} day-summary OHLCV succeed.");
                    return new OHLCV
                    {
                        Exchange = ExchangeNameEnum.MercadoBitcoin,
                        Ticker = ticker,
                        DateTime = dt,
                        Open = OHLCVFromApi.opening,
                        High = OHLCVFromApi.highest,
                        Low = OHLCVFromApi.lowest,
                        Close = OHLCVFromApi.closing,
                        Volume = OHLCVFromApi.volume
                    };
                }
                else
                {
                    string net_http_message_error_response = $"Exchange returned an error: {OHLCVFromApi.error}.";
                    _logger.LogError($"Get {ticker.ToString()} day-summary OHLCV failed:{net_http_message_error_response}");
                    throw new HttpRequestException(net_http_message_error_response);
                }
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                string net_http_message_not_success_statuscode = @"Response status code does not indicate success: {0} ({1}) - BODY: {2}.";
                string exceptionMessage = String.Format(net_http_message_not_success_statuscode, response.StatusCode, response.ReasonPhrase, responseBody);
                _logger.LogError($"Get {ticker.ToString()} day-summary OHLCV failed:\n {exceptionMessage}");
                throw new HttpRequestException(exceptionMessage);
            }

        }

        public async Task<OrderBook> GetOrderbookAsync(AssetTickerEnum ticker)
        {
            Uri requestUri = new Uri($"{ticker.ToString()}/orderbook/", UriKind.Relative);
            var response = await _client.GetAsync(requestUri);

            if(response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var orderBookFromApi = await JsonSerializer.DeserializeAsync<OrderbookDTO>(responseStream, _jsonOptions);
                if(String.IsNullOrEmpty(orderBookFromApi.error))
                {
                    _logger.LogInformation($"Get orderbook for {ticker.ToString()} succeed.");
                    return new OrderBook
                    {
                        Bids = orderBookFromApi.bids,
                        Asks = orderBookFromApi.asks
                    };
                }
                else
                {
                    string net_http_message_error_response = $"Exchange returned an error: {orderBookFromApi.error}.";
                    _logger.LogError($"Get {ticker.ToString()} day-summary OHLCV failed:{net_http_message_error_response}");
                    throw new HttpRequestException(net_http_message_error_response);
                }
            }
            else
            {
                _logger.LogError($"Get orderbook for {ticker.ToString()} failed.");
                string responseBody = await response.Content.ReadAsStringAsync();
                string net_http_message_not_success_statuscode = @"Response status code does not indicate success: {0} ({1}) - BODY: {2}.";
                throw new HttpRequestException(String.Format(net_http_message_not_success_statuscode, response.StatusCode, response.ReasonPhrase, responseBody));
            }

        }

        public async Task<Order[]> GetTradesSinceTIDAsync(AssetTickerEnum ticker, int tid)
        {
            Uri requestUri = new Uri($"{ticker.ToString()}/trades/{tid.ToString()}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var tradesFromApi = await JsonSerializer.DeserializeAsync<TradesDTO>(responseStream, _jsonOptions);
                if (String.IsNullOrEmpty(tradesFromApi.error))
                {
                    _logger.LogInformation($"Get trades for {ticker.ToString()} succeed.");
                    /*
                    var orders = new Order[tradesFromApi.Length];
                    int index = 0;
                    foreach (var trade in tradesFromApi)
                    {
                        OrderType type = trade.type.Equals("sell") ? OrderType.MARKET_SELL : OrderType.MARKET_BUY;
                        orders[index] = new Order(ticker, type, trade.amount, trade.price);
                        index++;
                    }
                    return orders;
                    */
                    return null;
                }
                else
                {
                    string net_http_message_error_response = $"Exchange returned an error: {tradesFromApi.error}.";
                    _logger.LogError($"Get {ticker.ToString()} day-summary OHLCV failed:{net_http_message_error_response}");
                    throw new HttpRequestException(net_http_message_error_response);
                }
            }
            else
            {
                _logger.LogError($"Get trades for {ticker.ToString()} failed.");
                string responseBody = await response.Content.ReadAsStringAsync();
                string net_http_message_not_success_statuscode = @"Response status code does not indicate success: {0} ({1}) - BODY: {2}.";
                throw new HttpRequestException(String.Format(net_http_message_not_success_statuscode, response.StatusCode, response.ReasonPhrase, responseBody));
            }

        }


    }
}
