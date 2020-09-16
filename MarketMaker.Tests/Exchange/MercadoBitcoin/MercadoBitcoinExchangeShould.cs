using MarketMaker.Core.Exchange;
using MarketMaker.Core.Models;
using MarketMaker.Exchange.MercadoBitcoin;
using MarketMaker.Exchange.MercadoBitcoin.Private;
using MarketMaker.Exchange.MercadoBitcoin.Public;
using MarketMaker.Exchange.MercadoBitcoin.Trade;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MarketMaker.Tests.Exchange.MercadoBitcoin
{
    public class MercadoBitcoinExchangeShould
    {
        private readonly IExchange _exchange;
        public MercadoBitcoinExchangeShould()
        {
            var logger = new Mock<ILogger<MercadoBitcoinExchange>>();
            var publicLogger = new Mock<ILogger<PublicApiClient>>();
            var privateLogger = new Mock<ILogger<PrivateApiClient>>();
            var tradeLogger = new Mock<ILogger<TradeApiClient>>();

            var publicClient = new PublicApiClient(new HttpClient(), publicLogger.Object);
            var privateClient = new PrivateApiClient(new HttpClient(), privateLogger.Object);
            var tradeClient = new TradeApiClient(new HttpClient(), tradeLogger.Object);
            var clientCredential = new Mock<IOptionsMonitor<ClientCredential>>();
            _exchange = new MercadoBitcoinExchange(publicClient, privateClient, tradeClient, logger.Object, clientCredential.Object);
        }

        [Fact]
        public async void FetchDaySummaryOHLCVByMarketAndDateTime()
        {
            //Arrange:
            DateTime dt = new DateTime(2013, 6, 20, 2, 40, 30);
            var market = new Market(AssetTicker.BTC, AssetTicker.BRL);
            var timeframe = new TimeFrame(TimeFrameEnum.D1);

            //Act:
            var response = await _exchange.FetchDaySummaryAsync(market, dt).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Succeed);
            Assert.NotNull(response.Output);
            Assert.Equal(ExchangeName.MercadoBitcoin, response.Output.Exchange);
            Assert.Equal(timeframe.Enum, response.Output.TimeFrame.Enum);
            Assert.Equal(market, response.Output.Market);
            Assert.Equal(new DateTime(2013, 6, 20), response.Output.DateTimeOffset);
            Assert.Equal((decimal)262.99999, response.Output.Open, 5);
            Assert.Equal((decimal)269.0, response.Output.High, 5);
            Assert.Equal((decimal)260.00002, response.Output.Low, 5);
            Assert.Equal((decimal)269.0, response.Output.Close, 5);
            Assert.Equal((decimal)7253.1336356785, response.Output.Volume, 5);
            Assert.Equal((decimal)27.11390588, response.Output.TradedQuantity, 5);
            Assert.Equal((decimal)267.5060416518087, response.Output.Average, 5);
            Assert.Equal((int)28, response.Output.NumberOfTrades);
        }

        [Fact]
        public async void FetchOrderBookByMarket()
        {
            //Arrange:
            var market = new Market(AssetTicker.BTC, AssetTicker.BRL);
            var currentTime = DateTimeUtils.CurrentUtcDateTimeOffset();
            var tolerance = TimeSpan.FromSeconds(30);

            //Act:
            var response = await _exchange.FetchOrderBookAsync(market).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Succeed);
            Assert.NotNull(response.Output);
            Assert.Equal(ExchangeName.MercadoBitcoin, response.Output.Exchange);
            Assert.Equal(currentTime.DateTime, response.Output.DateTimeOffset.DateTime, tolerance);
            Assert.Equal(market, response.Output.Market);
            Assert.NotNull(response.Output.Asks);
            Assert.Equal(1000, response.Output.Asks.Count());
            Assert.InRange<decimal>(response.Output.Asks.First().Price, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Asks.First().Amount, 0, 50);
            Assert.NotNull(response.Output.Bids);
            Assert.Equal(1000, response.Output.Bids.Count());
            Assert.InRange<decimal>(response.Output.Bids.First().Price, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Bids.First().Amount, 0, 50);
        }

        [Fact]
        public async void FetchOHLCVByMarket()
        {
            //Arrange:
            var market = new Market(AssetTicker.BTC, AssetTicker.BRL);
            var dt = DateTimeOffset.UtcNow;
            var tolerance = TimeSpan.FromMinutes(30);

            //Act:
            var response = await _exchange.FetchOHLCVAsync(market).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Succeed);
            Assert.NotNull(response.Output);
            Assert.Equal(dt.DateTime, response.Output.DateTimeOffset.DateTime, tolerance);
            Assert.InRange<decimal>(response.Output.Buy, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Sell, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.High, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Low, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Last, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Volume, 0, 1000);
        }
    }
}
