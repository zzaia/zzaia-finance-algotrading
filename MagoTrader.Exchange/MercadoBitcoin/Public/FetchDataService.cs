using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;
using MagoTrader.Exchange.MercadoBitcoin;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
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
        public async Task<OHLCV> GetPriceByTickerAsync(AssetTicker ticker, DateTime dt)
        {
            var temp = await JsonSerializer.DeserializeAsync<DataJson>
                (await _httpClient.GetStreamAsync($"{ticker.ToString()}/day-summary/{dt.Year}/{dt.Month}/{dt.Day-1}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            //Console.WriteLine($"{temp.opening}");
            return new OHLCV {
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
