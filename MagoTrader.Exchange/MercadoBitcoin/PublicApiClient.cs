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

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class PublicApiClient : IPublicApiClient
    {
        protected readonly ApiOptions _apiOptions;
        protected readonly ILogger<PublicApiClient> _logger;
        protected HttpClient _client;

        private JsonSerializerOptions _jsonOptions;
        public PublicApiClient( HttpClient client, IOptionsMonitor<ApiOptions> apiOptions, ILogger<PublicApiClient> logger)
        {
            _apiOptions = apiOptions?.Get("KycApi") ?? throw new ArgumentNullException(nameof(apiOptions));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _client.BaseAddress = _apiOptions.PublicBaseAddress ?? new Uri("https://www.mercadobitcoin.net/api/");
            _jsonOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }
        
        public async Task<OHLCV> GetDaySummaryOHLCVAsync(AssetTicker ticker, DateTime dt)
        {
            Uri requestUri = new Uri($"{ticker.ToString()}/day-summary/{dt.Year}/{dt.Month}/{dt.Day}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri);

            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Get {ticker.ToString()} day-summary OHLCV succeed.");
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var OHLCVFromApi = await JsonSerializer.DeserializeAsync<ohlcv>(responseStream, _jsonOptions);
                return new OHLCV
                {
                    Exchange = ExchangeName.MercadoBitcoin,
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
                _logger.LogError($"Get {ticker.ToString()} day-summary OHLCV failed.");
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"{_apiOptions.Name.ToString()} response body: {responseBody}.");
                string net_http_message_not_success_statuscode = @"Response status code does not indicate success: {0} ({1}) - BODY: {2}.";
                throw new HttpRequestException(String.Format(net_http_message_not_success_statuscode, response.StatusCode, response.ReasonPhrase, responseBody));
            }

        }

        public async Task<OrderBook> GetOrderbookAsync(AssetTicker ticker)
        {
            Uri requestUri = new Uri($"{ticker.ToString()}/orderbook/", UriKind.Relative);
            var response = await _client.GetAsync(requestUri);

            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Get orderbook for {ticker.ToString()} succeed.");
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var orderBookFromApi = await JsonSerializer.DeserializeAsync<orderbook>(responseStream, _jsonOptions);
                return new OrderBook
                {
                    Bids = orderBookFromApi.bids,
                    Asks = orderBookFromApi.asks
                };
            }
            else
            {
                _logger.LogError($"Get orderbook for {ticker.ToString()} failed.");
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"{_apiOptions.Name.ToString()} response body: {responseBody}.");
                string net_http_message_not_success_statuscode = @"Response status code does not indicate success: {0} ({1}) - BODY: {2}.";
                throw new HttpRequestException(String.Format(net_http_message_not_success_statuscode, response.StatusCode, response.ReasonPhrase, responseBody));
            }

        }

        public async Task<Order[]> GetTradesSinceTIDAsync(AssetTicker ticker, int tid)
        {
            Uri requestUri = new Uri($"{ticker.ToString()}/trades/{tid.ToString()}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"Get trades for {ticker.ToString()} succeed.");
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var tradesFromApi = await JsonSerializer.DeserializeAsync<trade[]>(responseStream, _jsonOptions);
                var orders = new Order[tradesFromApi.Length];
                int index = 0;
                foreach(var trade in tradesFromApi)
                {
                    OrderType type = trade.type.Equals("sell") ? OrderType.MARKET_SELL : OrderType.MARKET_BUY;
                    orders[index] = new Order(ticker, type, trade.amount, trade.price);
                    index++;
                }
                return orders;
            }
            else
            {
                _logger.LogError($"Get trades for {ticker.ToString()} failed.");
                string responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"{_apiOptions.Name.ToString()} response body: {responseBody}.");
                string net_http_message_not_success_statuscode = @"Response status code does not indicate success: {0} ({1}) - BODY: {2}.";
                throw new HttpRequestException(String.Format(net_http_message_not_success_statuscode, response.StatusCode, response.ReasonPhrase, responseBody));
            }

        }




    }
}
