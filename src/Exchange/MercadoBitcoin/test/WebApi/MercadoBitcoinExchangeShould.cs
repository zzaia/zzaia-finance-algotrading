using Zzaia.Finance.Core.Interfaces.ExchangeAggregate;
using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.ExchangeAggregate;
using Zzaia.Finance.Core.Models.MarketAgregate;
using Zzaia.Finance.Core.Utils;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Test
{
    public class MercadoBitcoinExchangeShould
    {
        private readonly IExchange _exchange;
        public MercadoBitcoinExchangeShould()
        {
            var logger = new Mock<ILogger<MercadoBitcoinExchange>>();

            var privateClientCredential = new Mock<Action<ClientCredential>>();
            var tradeClientCredential = new Mock<Action<ClientCredential>>();
            var config = TelemetryConfiguration.CreateDefault();
            var telemetryClient = new TelemetryClient(config);
            var httpFactory = new Mock<IHttpClientFactory>();
            httpFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                       .Returns(() =>
                       {
                           var client = new HttpClient();
                           return client;
                       });
            _exchange = new MercadoBitcoinExchange(privateClientCredential.Object,
                                                   tradeClientCredential.Object,
                                                   logger.Object,
                                                   telemetryClient,
                                                   httpFactory.Object);
        }

        [Fact]
        public async void FetchOrderBookByMarket()
        {
            //Arrange:
            var market = new Market(Asset.BTC, Asset.BRL);
            var currentTime = DateTimeUtils.CurrentUtcDateTimeOffset();
            var tolerance = TimeSpan.FromSeconds(30);
            var cancellationToken = new CancellationToken();

            //Act:
            var response = await _exchange.FetchOrderBookAsync(market, cancellationToken).ConfigureAwait(true);

            //Assert:
            Assert.True(response.Succeed);
            Assert.NotNull(response.Output);
            Assert.Equal(ExchangeName.MercadoBitcoin, response.Output.Exchange);
            Assert.Equal(currentTime.DateTime, response.Output.ServerTimeStamp.DateTime, tolerance);
            Assert.Equal(market, response.Output.Market);
            Assert.NotNull(response.Output.Asks);
            Assert.Equal(1000, response.Output.Asks.Count());
            Assert.InRange(response.Output.Asks.First().Price, 10000, decimal.MaxValue);
            Assert.InRange(response.Output.Asks.First().Amount, 0, 50);
            Assert.NotNull(response.Output.Bids);
            Assert.Equal(1000, response.Output.Bids.Count());
            Assert.InRange(response.Output.Bids.First().Price, 10000, decimal.MaxValue);
            Assert.InRange(response.Output.Bids.First().Amount, 0, 50);
        }
    }
}