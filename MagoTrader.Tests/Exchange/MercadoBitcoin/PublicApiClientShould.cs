using System;
using Xunit;
using MagoTrader.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagoTrader.Core.Exchange;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using MagoTrader.Exchange.MercadoBitcoin.Public;

namespace MagoTrader.Tests.Exchange.MercadoBitcoin
{
    public class PublicApiClientShould
    {
        protected readonly PublicApiClient _client;
        public PublicApiClientShould()
        {
            var htttpClient = new HttpClient();
            var logger = new Logger<PublicApiClient>(new LoggerFactory());
            _client = new PublicApiClient(htttpClient, logger);
        }
        
        [Fact]
        public async void GetDaySummaryOHLCVByMarketAndDateTime()
        {
            //Arrange:
            DateTime dt = new DateTime(2013,6,20,2,40,30);
            var market = new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL);
            var timeframe = new TimeFrame(TimeFrameEnum.D1);

            //Act:
            OHLCV result = await _client.GetDaySummaryOHLCVAsync(market, dt);

            //Assert:
            Assert.Equal(ExchangeNameEnum.MercadoBitcoin, result.Exchange);
            Assert.Equal(timeframe.Enum, result.TimeFrame.Enum);
            Assert.Equal(market, result.Market);
            Assert.Equal(new DateTime(2013, 6, 20), result.DateTime);
            Assert.Equal((decimal)262.99999, result.Open,5);
            Assert.Equal((decimal)269.0, result.High,5);
            Assert.Equal((decimal)260.00002, result.Low,5);
            Assert.Equal((decimal)269.0, result.Close,5);
            Assert.Equal((decimal)7253.1336356785, result.Volume,5);
            Assert.Equal((decimal)27.11390588, result.TradedQuantity,5);
            Assert.Equal((decimal)267.5060416518087, result.Average,5);
            Assert.Equal((int)28, result.NumberOfTrades);
        }

        [Fact]
        public async void GetOrderBookByMarket()
        {
            //Arrange:
            var market = new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL);

            //Act:
            OrderBook result = await _client.GetOrderbookAsync(market);

            //Assert:
            Assert.NotNull(result.Asks);
            Assert.Equal(1000, result.Asks.Length);
            Assert.InRange<decimal>(result.Asks[0][0], 10000, decimal.MaxValue);
            Assert.InRange<decimal>(result.Asks[0][1], 0, 50);
            Assert.NotNull(result.Bids);
            Assert.Equal(1000, result.Bids.Length);
            Assert.InRange<decimal>(result.Bids[0][0], 10000, decimal.MaxValue);
            Assert.InRange<decimal>(result.Bids[0][1], 0, 50);
        }

    }
}
