using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.MarketAgregate;
using Zzaia.Finance.Core.Utils;
using Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Public;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Test
{
    public class PublicApiClientShould
    {
        private readonly PublicApiClient _client;
        public PublicApiClientShould()
        {
            var htttpClient = new HttpClient();
            _client = new PublicApiClient(htttpClient);
            _client.SetBaseAddress(new Uri("https://www.mercadobitcoin.net/api/"));
        }

        [Fact]
        public async void GetDaySummaryOHLCVByMarketAndDateTime()
        {
            //Arrange:
            DateTime dt = new DateTime(2013, 6, 20, 2, 40, 30);
            var market = new Market(Asset.BTC, Asset.BRL);
            var cancellationToken = new CancellationToken();

            //Act:
            var response = await _client.GetDaySummaryOHLCVAsync(market.Base.ToString(), dt.Year, dt.Month, dt.Day, cancellationToken).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.Equal((decimal)262.99999, response.Output.Open, 5);
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
        public async void GetLast24hOHLCVByMainTicker()
        {
            //Arrange:
            var market = new Market(Asset.BTC, Asset.BRL);
            var dt = DateTimeOffset.UtcNow;
            var tolerance = TimeSpan.FromMinutes(30);
            var cultureInfo = new CultureInfo("en-us");
            var cancellationToken = new CancellationToken();

            //Act:
            var response = await _client.GetLast24hOHLCVAsync(market.Base.ToString(), cancellationToken).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.Equal(dt.DateTime, DateTimeUtils.TimestampToDateTimeOffset(response.Output.Ticker.TimeStamp, false).DateTime, tolerance);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Output.Ticker.Buy, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Output.Ticker.Sell, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Output.Ticker.High, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Output.Ticker.Low, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Output.Ticker.Last, cultureInfo), 10000, decimal.MaxValue);
            Assert.InRange<decimal>(Convert.ToDecimal(response.Output.Ticker.Volume, cultureInfo), 0, 1000);
        }

        [Fact]
        public async void GetOrderBookByMainTicker()
        {
            //Arrange:
            var market = new Market(Asset.BTC, Asset.BRL);
            var cancellationToken = new CancellationToken();

            //Act:
            var response = await _client.GetOrderBookAsync(market.Base.ToString(), cancellationToken).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.NotNull(response.Output.Asks);
            Assert.Equal(1000, response.Output.Asks.Length);
            Assert.InRange<decimal>(response.Output.Asks[0][0], 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Asks[0][1], 0, 50);
            Assert.NotNull(response.Output.Bids);
            Assert.Equal(1000, response.Output.Bids.Length);
            Assert.InRange<decimal>(response.Output.Bids[0][0], 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Bids[0][1], 0, 50);
        }
        [Fact]
        public async void GetTradesSinceTIDByMainTicker()
        {
            //Arrange:
            var market = new Market(Asset.BTC, Asset.BRL);
            string tid = "5700";
            //var globalId = new Guid(tid.GetHashCode().ToString());
            var cancellationToken = new CancellationToken();

            //Act:
            var response = await _client.GetTradesSinceTIDAsync(market.Base.ToString(), tid, cancellationToken).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
        }

    }
}
