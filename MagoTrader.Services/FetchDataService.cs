using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;
using MagoTrader.Exchange.MercadoBitcoin;

namespace MagoTrader.Services
{
    public class FetchDataService : IFetchDataService
    {
        private readonly HttpClient _httpClient;
        private AssetTicker[] _tickers;

        public FetchDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _tickers = new[] {AssetTicker.BTC, AssetTicker.ETH, AssetTicker.LTC, AssetTicker.XRP, AssetTicker.BCH};
        }
        public async Task<OHLCV[]> GetForecastAsync(DateTime dt)
        {
            List<Task<OHLCV>> tasks = new List<Task<OHLCV>>();
            OHLCV[] data = new OHLCV[_tickers.Length];
            foreach(var tck in _tickers)
            {
                tasks.Add(Task.Run(() => GetPriceByTickerAsync(tck, dt)));
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


        public async Task<Decimal> FetchLastPriceAsync(AssetTicker ticker)
        {
            var temp = await JsonSerializer.DeserializeAsync<JsonDataFormat>
                (await _httpClient.GetStreamAsync($"{ticker.ToString()}/ticker/"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return temp.ticker.last;

        }

        public async Task<OHLCV> Fetch24hOHLCVAsync(AssetTicker ticker, DateTime dt)
        {
            var temp = await JsonSerializer.DeserializeAsync<JsonDataFormat>
                (await _httpClient.GetStreamAsync($"{ticker.ToString()}/day-summary/{dt.Year}/{dt.Month}/{dt.Day}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return new OHLCV {
                //Exchange = SupportedExchanges.MercadoBitcoin,
                Ticker = ticker, 
                DateTime = dt,
                Open = temp.opening,
                High = temp.highest,
                Low = temp.lowest,
                Close = temp.closing,
                Volume = temp.volume
            };
        }

        public async Task<OHLCV> GetPriceByTickerAsync(AssetTicker ticker, DateTime dt)
        {
            var temp = await JsonSerializer.DeserializeAsync<JsonDataFormat>
                (await _httpClient.GetStreamAsync($"{ticker.ToString()}/day-summary/{dt.Year}/{dt.Month}/{dt.Day-1}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //Console.WriteLine($"{temp.opening}");
            return new OHLCV {
                //Exchange = SupportedExchanges.MercadoBitcoin,
                Ticker = ticker, 
                DateTime = dt,
                Open = temp.opening,
                High = temp.highest,
                Low = temp.lowest,
                Close = temp.closing,
                Volume = temp.volume
            };
        }
    }
}
