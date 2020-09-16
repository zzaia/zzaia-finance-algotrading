using MarketMaker.Core.Exchange;
using MarketMaker.Core.Models;
using MarketMaker.Core.Repositories;
using MarketMaker.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketMaker.Services
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
            try
            {
                var exchange = _exchangeSelector.GetByName(exchangeName);
                Market[] markets = exchange.Info.Markets.ToArray();
                List<Task<ObjectResult<OHLCV>>> tasks = new List<Task<ObjectResult<OHLCV>>>();
                OHLCV[] data = new OHLCV[markets.Length];
                foreach (var mkt in markets)
                {
                    tasks.Add(Task.Run(() => exchange.FetchDaySummaryAsync(mkt, dt)));
                    //tasks.Add(exchange.FetchDaySummaryAsync(mkt, dt));
                }
                await Task.WhenAll(tasks);
                for (int i = 0; i < tasks.Count; i++)
                {
                    data[i] = tasks[i].Result.Output;
                }
                return data;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }
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
