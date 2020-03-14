using System;
using Xunit;
using  MagoTrader.Exchange.MercadoBitcoin.Public;

namespace MagoTrader.Tests.Exchange
{
    public class MercadoBitcoinTests
    {
        private AssetTicker[] _tickers;
        public MercadoBitcoinTests()
        {
            _tickers = new[] {AssetTicker.BTC, AssetTicker.ETH, AssetTicker.LTC, AssetTicker.XRP, AssetTicker.BCH};
        }
        [Fact]
        public async void FetchTickerPriceByDateTime()
        {
            List<Task<OHLCV>> tasks = new List<Task<OHLCV>>();
            Decimal[] data = new Decimal[_tickers.Length];
            //Act:
            foreach(var tck in _tickers)
            {
                tasks.Add(Task.Run(() => PriceAsync(tck, dt)));
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
            //Assert:
            Assert.Equal(12.4,data,1);
            Assert.Equal(98.2,data.Low,1);
            Assert.Equal(47.85,Data.Close,1);
            Assert.Equal('F',book.Stats.Letter);
            Assert.Equal('F',book.Stats.Letter);
        }
    }
}
