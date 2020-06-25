using MagoTrader.Core.Models;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Net.Http;
using Xunit;

namespace MagoTrader.Tests.Exchange.MercadoBitcoin
{
    public class PublicApiClientShould
    {
        private readonly PublicApiClient _client;
        public PublicApiClientShould()
        {
            var htttpClient = new HttpClient();
            var logger = new Logger<PublicApiClient>(new LoggerFactory());
            _client = new PublicApiClient(htttpClient, logger);
            _client.SetBaseAddress(new Uri("https://www.mercadobitcoin.net/api/"));
        }

        [Fact]
        public async void GetDaySummaryOHLCVByMarketAndDateTime()
        {
            //Arrange:
            DateTime dt = new DateTime(2013, 6, 20, 2, 40, 30);
            var market = new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL);

            //Act:
            var response = await _client.GetDaySummaryOHLCVAsync(market.Main.ToString(), dt.Year, dt.Month, dt.Day).ConfigureAwait(true);

            //Assert:
            Assert.True(response.IsOK());
            Assert.NotNull(response);
            Assert.Equal((decimal)262.99999, response.Value.Open, 5);
            Assert.Equal((decimal)262.99999, response.Value.Open, 5);
            Assert.Equal((decimal)269.0, response.Value.High, 5);
            Assert.Equal((decimal)260.00002, response.Value.Low, 5);
            Assert.Equal((decimal)269.0, response.Value.Close, 5);
            Assert.Equal((decimal)7253.1336356785, response.Value.Volume, 5);
            Assert.Equal((decimal)27.11390588, response.Value.TradedQuantity, 5);
            Assert.Equal((decimal)267.5060416518087, response.Value.Average, 5);
            Assert.Equal((int)28, response.Value.NumberOfTrades);

        }

        [Fact]
        public async void GetLast24hOHLCVByMainTicker()
        {
            //Arrange:
            var market = new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL);
            var dt = DateTimeOffset.UtcNow;
            var tolerance = TimeSpan.FromMinutes(30);
            var cultureInfo = new CultureInfo("en-us");
            //Act:
            var response = await _client.GetLast24hOHLCVAsync(market.Main.ToString()).ConfigureAwait(true);

            //Assert:
            Assert.True(response.IsOK());
            Assert.NotNull(response);
            Assert.Equal(dt.DateTime, DateTimeUtils.TimestampToDateTimeOffset(response.Value.Ticker.TimeStamp, false).DateTime, tolerance);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Value.Ticker.Buy, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Value.Ticker.Sell, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Value.Ticker.High, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Value.Ticker.Low, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Value.Ticker.Last, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Value.Ticker.Volume, cultureInfo), 0, 1000);
        }

        [Fact]
        public async void GetOrderBookByMainTicker()
        {
            //Arrange:
            var market = new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL);

            //Act:
            var response = await _client.GetOrderBookAsync(market.Main.ToString()).ConfigureAwait(true);

            //Assert:
            Assert.True(response.IsOK());
            Assert.NotNull(response);
            Assert.NotNull(response.Value.Asks);
            Assert.Equal(1000, response.Value.Asks.Length);
            Assert.InRange<decimal>(response.Value.Asks[0][0], 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Value.Asks[0][1], 0, 50);
            Assert.NotNull(response.Value.Bids);
            Assert.Equal(1000, response.Value.Bids.Length);
            Assert.InRange<decimal>(response.Value.Bids[0][0], 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Value.Bids[0][1], 0, 50);
        }
        [Fact]
        public async void GetTradesSinceTIDByMainTicker()
        {
            //Arrange:
            var market = new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL);
            string tid = "5700";
            //var globalId = new Guid(tid.GetHashCode().ToString());

            //Act:
            var response = await _client.GetTradesSinceTIDAsync(market.Main.ToString(), tid).ConfigureAwait(true);

            //Assert:
            Assert.True(response.IsOK());
            Assert.NotNull(response);
        }

    }
}
