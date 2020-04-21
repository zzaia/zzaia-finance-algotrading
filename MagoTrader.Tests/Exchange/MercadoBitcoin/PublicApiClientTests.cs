using System;
using Xunit;
using MagoTrader.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagoTrader.Core.Exchange;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using MagoTrader.Exchange;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Exchange.MercadoBitcoin.Private;
using MagoTrader.Exchange.MercadoBitcoin.Trade;
using MagoTrader.Exchange.MercadoBitcoin;
using Microsoft.Extensions.Options;

namespace MagoTrader.Tests.Exchange.MercadoBitcoin
{
    public class PublicApiClientTests
    {
        private AssetTickerEnum[] _tickers;
        protected readonly ILogger<PublicApiClientTests> _logger;
        private PublicApiClient _client;
        private IExchange _exchange;
        public PublicApiClientTests()
        {
            var loggerFactory = new LoggerFactory();
            var httpClient = new HttpClient();
            _logger = new Logger<PublicApiClientTests>(loggerFactory);
            _client = new PublicApiClient(httpClient,new Logger<PublicApiClient>(loggerFactory));
            _exchange = new MagoTrader.Exchange.MercadoBitcoin.MercadoBitcoin(_client,null,null);
            _tickers = new[] {AssetTickerEnum.BTC, AssetTickerEnum.ETH, AssetTickerEnum.LTC, AssetTickerEnum.XRP, AssetTickerEnum.BCH};

        }
        /*
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
        */
    }
}
