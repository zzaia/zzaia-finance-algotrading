using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;
using MagoTrader.Core.Exchange;
using MagoTrader.Exchange;
using Microsoft.Extensions.Logging;
using MagoTrader.Exchange.MercadoBitcoin;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Core.Services;

namespace MagoTrader.Services
{
    public class FetchDataService : IFetchDataService
    {
        private readonly IExchangeSelector _exchangeSelector;
        private readonly ILogger<FetchDataService> _logger;

        public FetchDataService(IExchangeSelector exchangeSelector, ILogger<FetchDataService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _exchangeSelector = exchangeSelector ?? throw new ArgumentNullException(nameof(exchangeSelector));
        }
        public async Task<OHLCV[]> GetDefaultDaySummaryAsync(DateTimeOffset dt, ExchangeNameEnum exchangeName)
        {
            var exchange = _exchangeSelector.GetByName(exchangeName);
            Market[] markets = exchange.Info.Markets.ToArray();
            List<Task<ObjectResult<OHLCV>>> tasks = new List<Task<ObjectResult<OHLCV>>>();
            OHLCV[] data = new OHLCV[markets.Length];
            foreach(var mkt in markets)
            {
                tasks.Add(Task.Run(() => exchange.FetchDaySummaryAsync(mkt, dt)));
                //tasks.Add( GetPriceByTickerAsync(tck, dt));
            }

            try {
                await Task.WhenAll(tasks);
                for (int i = 0; i < tasks.Count; i++) 
                {
                    data[i] = tasks[i].Result.Output;
                }
            }
            catch(Exception e){ _logger.LogError(e.Message); }
            return data;
        }

        /*
        public async Task<Decimal[]> GetLastPriceStreamAsync()
        {
            List<Task<Decimal>> tasks = new List<Task<Decimal>>();
            Decimal[] data = new Decimal[_tickers.Length];
            foreach(var tck in _tickers)
            {
                tasks.Add(Task.Run(() => FetchLastPriceAsync(tck)));
                //tasks.Add( GetPriceByTickerAsync(tck, dt));
            }

            try {
                await Task.WhenAll(tasks);
                for (int i = 0; i < tasks.Count; i++) 
                {
                    data[i] = tasks[i].Result;
                }
            }
            catch(AggregateException){}
            return data;
        }
        */
    }
}
