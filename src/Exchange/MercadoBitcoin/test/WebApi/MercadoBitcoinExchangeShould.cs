using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Utils;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Test
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
            Assert.Equal(currentTime.DateTime, response.Output.DateTimeOffset.DateTime, tolerance);
            Assert.Equal(market, response.Output.Market);
            Assert.NotNull(response.Output.Asks);
            Assert.Equal(1000, response.Output.Asks.Count());
            Assert.InRange<decimal>(response.Output.Asks.First().Item2, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Asks.First().Item1, 0, 50);
            Assert.NotNull(response.Output.Bids);
            Assert.Equal(1000, response.Output.Bids.Count());
            Assert.InRange<decimal>(response.Output.Bids.First().Item2, 10000, decimal.MaxValue);
            Assert.InRange<decimal>(response.Output.Bids.First().Item1, 0, 50);
        }
    }
}