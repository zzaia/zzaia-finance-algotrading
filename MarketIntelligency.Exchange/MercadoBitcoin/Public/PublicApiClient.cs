using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.Models.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin.Public
{
    public class PublicApiClient : ApiClientBase, IPublicApiClient
    {
        private readonly HttpClient _client;
        private readonly bool _continueOnCapturedContext;

        public PublicApiClient(HttpClient client, ILogger<PublicApiClient> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _continueOnCapturedContext = true;
        }

        public void SetBaseAddress(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }

        public async Task<Response<OHLCVDTO>> GetDaySummaryOHLCVAsync(string mainTicker, int year, int month, int day, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get {mainTicker} day-summary OHLCV.");
            Uri requestUri = new Uri($"{mainTicker}/day-summary/{year}/{month}/{day}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<OHLCVDTO>(response).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<TickerDataDTO>> GetLast24hOHLCVAsync(string mainTicker, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get {mainTicker} last 24h OHLCV.");
            Uri requestUri = new Uri($"{mainTicker}/ticker/", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<TickerDataDTO>(response).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<OrderBookDTO>> GetOrderBookAsync(string mainTicker, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get orderbook for {mainTicker}.");
            Uri requestUri = new Uri($"{mainTicker}/orderbook/", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<OrderBookDTO>(response).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<IEnumerable<TradeDTO>>> GetTradesSinceTIDAsync(string mainTicker, string tid, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get trades since {tid} for {mainTicker}.");
            Uri requestUri = new Uri($"{mainTicker}/trades/?since={tid}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<IEnumerable<TradeDTO>>(response).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<IEnumerable<TradeDTO>>> GetLastTradesAsync(string mainTicker, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get last trades for {mainTicker}.");
            Uri requestUri = new Uri($"{mainTicker}/trades/", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<IEnumerable<TradeDTO>>(response).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<IEnumerable<TradeDTO>>> GetTradesFromTimeStampAsync(string mainTicker, int timeStamp, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get trades from {timeStamp} for {mainTicker}.");
            Uri requestUri = new Uri($"{mainTicker}/trades/{timeStamp}/", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<IEnumerable<TradeDTO>>(response).ConfigureAwait(_continueOnCapturedContext);
        }

        public async Task<Response<IEnumerable<TradeDTO>>> GetTradesBetweenTimeStampAsync(string mainTicker, int fromTimeStamp, int toTimeStamp, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Get trades from {fromTimeStamp} to {toTimeStamp} for {mainTicker}.");
            Uri requestUri = new Uri($"{mainTicker}/trades/{fromTimeStamp}/{toTimeStamp}", UriKind.Relative);
            var response = await _client.GetAsync(requestUri, cancellationToken).ConfigureAwait(_continueOnCapturedContext);
            return await this.GetResponseAsync<IEnumerable<TradeDTO>>(response).ConfigureAwait(_continueOnCapturedContext);
        }
    }
}