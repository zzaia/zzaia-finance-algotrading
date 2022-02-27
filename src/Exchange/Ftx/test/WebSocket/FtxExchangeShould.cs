using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net.Http;
using Xunit;

namespace MarketIntelligency.Exchange.Ftx.Test
{
    public class FtxExchangeShould
    {
        private readonly IExchange _exchange;
        public FtxExchangeShould()
        {
            var logger = new Mock<ILogger<FtxExchange>>();

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
            _exchange = new FtxExchange(privateClientCredential.Object,
                                                   tradeClientCredential.Object,
                                                   logger.Object,
                                                   telemetryClient,
                                                   httpFactory.Object);
        }

        [Fact]
        public void ReceiveOrderBookSnapShot()
        {

            //_exchange.ReceiveAsync(It.IsAny<string>(),  
        }
    }
}
