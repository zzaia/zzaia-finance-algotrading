using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.Exchange.Ftx.WebSocket.Models;
using MarketIntelligency.WebSocket;
using MarketIntelligency.WebSocket.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MarketIntelligency.Exchange.Ftx.Test
{
    public class FtxExchangeShould
    {

        [Fact]
        public async Task ReceiveOrderBookSnapShot()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var logger = new Mock<ILogger<FtxExchange>>();
            var privateClientCredential = new Mock<Action<ClientCredential>>();
            var tradeClientCredential = new Mock<Action<ClientCredential>>();
            var config = TelemetryConfiguration.CreateDefault();
            var telemetryClient = new TelemetryClient(config);
            var httpFactory = new Mock<IHttpClientFactory>();
            var webSocketClient = new Mock<IWebSocketClient>();
            httpFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                       .Returns(() =>
                       {
                           var client = new HttpClient();
                           return client;
                       });
            var exchange = new FtxExchange(privateClientCredential.Object,
                                           tradeClientCredential.Object,
                                           logger.Object,
                                           telemetryClient,
                                           httpFactory.Object,
                                           webSocketClient.Object);


            //Setup
            var payload = "{\"channel\": \"orderbook\", \"market\": \"BTC/BRZ\", \"type\": \"partial\", \"data\": {\"time\": 1646001977.62839, \"checksum\": 2117160094, \"bids\": [[196405.0, 0.0537], [196400.0, 0.01], [196230.0, 0.013], [196050.0, 0.9598], [196000.0, 0.01], [195870.0, 0.012], [195850.0, 0.398], [195470.0, 0.402], [195345.0, 0.6237], [195155.0, 0.4951], [194680.0, 0.8431], [194510.0, 0.6895], [193925.0, 0.0011], [193920.0, 0.7346], [193735.0, 0.9285], [193715.0, 1.0644], [193540.0, 1.1826], [192890.0, 0.9562], [192560.0, 0.0077], [192495.0, 49.9396], [192390.0, 0.0019], [190800.0, 49.3892], [190740.0, 0.0064], [190470.0, 0.0009], [190000.0, 0.0092], [189310.0, 0.0092], [188340.0, 0.0005], [188040.0, 0.011], [188000.0, 0.0046], [186535.0, 0.0031], [186090.0, 0.0007], [185000.0, 0.0067], [183295.0, 84.8299], [181510.0, 82.8566], [180720.0, 0.0055], [180500.0, 0.0002], [180350.0, 0.0234], [180000.0, 0.0103], [179250.0, 0.015], [179000.0, 0.0047], [178500.0, 0.0002], [175165.0, 0.003], [175000.0, 0.0057], [174325.0, 0.0005], [173850.0, 0.0008], [173750.0, 0.0005], [173140.0, 103.9934], [172510.0, 0.0002], [172410.0, 0.0002], [171510.0, 0.0004], [171410.0, 0.0002], [171110.0, 0.0035], [170665.0, 0.0001], [170510.0, 0.0002], [170010.0, 0.0002], [170000.0, 0.0115], [169135.0, 0.0011], [168580.0, 0.0005], [168385.0, 0.0004], [167580.0, 0.001], [165580.0, 0.0005], [165410.0, 0.0005], [164755.0, 0.0001], [164660.0, 0.0035], [163580.0, 0.0005], [161410.0, 0.0006], [161200.0, 0.0031], [161110.0, 0.0038], [160665.0, 0.0001], [160580.0, 0.0005], [160000.0, 0.0313], [158635.0, 130.0812], [156965.0, 0.0216], [156920.0, 0.0036], [154510.0, 0.0003], [154500.0, 0.0032], [152110.0, 0.0038], [150000.0, 0.0117], [149180.0, 0.0126], [146500.0, 0.0024], [142035.0, 0.0054], [140000.0, 0.0035], [17600.0, 0.1136], [17400.0, 0.2298], [185.0, 0.082]], \"asks\": [[197490.0, 0.2056], [197495.0, 0.2643], [197500.0, 0.8831], [197505.0, 0.011], [197545.0, 1.7548], [197735.0, 0.009], [198000.0, 0.01], [198005.0, 0.6497], [198200.0, 0.5556], [198420.0, 0.575], [198485.0, 0.653], [198505.0, 0.6791], [198625.0, 0.015], [199000.0, 0.6638], [199310.0, 1.1022], [200010.0, 55.9356], [200050.0, 0.9599], [200200.0, 1.2584], [200300.0, 1.0629], [202330.0, 0.0025], [209970.0, 91.68], [218670.0, 0.0007], [220000.0, 0.0009], [228790.0, 102.9165], [229530.0, 110.9495], [230000.0, 0.005], [231340.0, 0.0021], [233550.0, 0.0005], [234915.0, 0.0002], [235310.0, 0.001], [237775.0, 0.0002], [239775.0, 0.0002], [240000.0, 0.001], [247420.0, 0.0002], [248235.0, 125.2265], [250295.0, 139.8331], [252275.0, 0.0146], [253655.0, 0.015], [254170.0, 0.001], [256150.0, 0.015], [256990.0, 0.015], [259670.0, 0.0001], [269250.0, 0.0006], [269670.0, 0.0001], [269675.0, 0.0002], [271870.0, 0.015], [272215.0, 0.015], [273575.0, 0.015], [275140.0, 0.015], [275460.0, 0.015], [275745.0, 0.015], [279670.0, 0.0001], [280920.0, 0.0146], [280945.0, 0.015], [281310.0, 0.015], [281355.0, 0.015], [281435.0, 0.015], [281535.0, 0.015], [281910.0, 0.015], [281925.0, 0.015], [282840.0, 0.015], [288650.0, 0.015], [289670.0, 0.0001], [290945.0, 0.015], [291300.0, 0.0009], [299670.0, 0.0001], [300000.0, 0.0113], [309295.0, 0.0002], [312455.0, 0.0007], [329755.0, 0.0007]], \"action\": \"partial\"}}";
            var lenght = 3233;
            var payloadAsByteArray = Encoding.ASCII.GetBytes(payload);
            var payloadResponse = JsonSerializer.Deserialize<WebSocketResponse>(payload);
            var orderbookResponse = JsonSerializer.Deserialize<OrderbookResponse>(payload);
            webSocketClient.Setup(o => o.ReceiveAsync(cancellationToken))
                           .Returns(Task.FromResult(new WebSocketClientResponse()
                           {
                               Message = payloadAsByteArray,
                               MessageType = WebSocketMessageType.Text,
                               Lenght = lenght,
                           }));
            dynamic result = new object();
            void PublishEvent<T>(T content) where T : class
            {
                result = content;
            }
            await exchange.ReceiveAsync(PublishEvent, cancellationToken);

            //Assert
            Assert.IsType<OrderBook>(result);
            OrderBook teste = result as OrderBook;
            Assert.Equal(orderbookResponse.Data.Asks.Count(), teste.Asks.ToList().Count);
            Assert.Equal(orderbookResponse.Data.Bids.Count(), teste.Bids.ToList().Count);
        }

        [Fact]
        public async Task ReceiveOrderBookUpdate_case01()
        {
            //Arrange
            var cancellationToken = new CancellationToken();
            var logger = new Mock<ILogger<FtxExchange>>();
            var privateClientCredential = new Mock<Action<ClientCredential>>();
            var tradeClientCredential = new Mock<Action<ClientCredential>>();
            var config = TelemetryConfiguration.CreateDefault();
            var telemetryClient = new TelemetryClient(config);
            var httpFactory = new Mock<IHttpClientFactory>();
            var webSocketClient = new Mock<IWebSocketClient>();
            httpFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
                       .Returns(() =>
                       {
                           var client = new HttpClient();
                           return client;
                       });
            var exchange = new FtxExchange(privateClientCredential.Object,
                                           tradeClientCredential.Object,
                                           logger.Object,
                                           telemetryClient,
                                           httpFactory.Object,
                                           webSocketClient.Object);


            //Setup
            var payload = "{\"channel\": \"orderbook\", \"market\": \"BTC/BRZ\", \"type\": \"partial\", \"data\": {\"time\": 1646001977.62839, \"checksum\": 2117160094, \"bids\": [[196405.0, 0.0537], [196400.0, 0.01], [196230.0, 0.013], [196050.0, 0.9598], [196000.0, 0.01], [195870.0, 0.012], [195850.0, 0.398], [195470.0, 0.402], [195345.0, 0.6237], [195155.0, 0.4951], [194680.0, 0.8431], [194510.0, 0.6895], [193925.0, 0.0011], [193920.0, 0.7346], [193735.0, 0.9285], [193715.0, 1.0644], [193540.0, 1.1826], [192890.0, 0.9562], [192560.0, 0.0077], [192495.0, 49.9396], [192390.0, 0.0019], [190800.0, 49.3892], [190740.0, 0.0064], [190470.0, 0.0009], [190000.0, 0.0092], [189310.0, 0.0092], [188340.0, 0.0005], [188040.0, 0.011], [188000.0, 0.0046], [186535.0, 0.0031], [186090.0, 0.0007], [185000.0, 0.0067], [183295.0, 84.8299], [181510.0, 82.8566], [180720.0, 0.0055], [180500.0, 0.0002], [180350.0, 0.0234], [180000.0, 0.0103], [179250.0, 0.015], [179000.0, 0.0047], [178500.0, 0.0002], [175165.0, 0.003], [175000.0, 0.0057], [174325.0, 0.0005], [173850.0, 0.0008], [173750.0, 0.0005], [173140.0, 103.9934], [172510.0, 0.0002], [172410.0, 0.0002], [171510.0, 0.0004], [171410.0, 0.0002], [171110.0, 0.0035], [170665.0, 0.0001], [170510.0, 0.0002], [170010.0, 0.0002], [170000.0, 0.0115], [169135.0, 0.0011], [168580.0, 0.0005], [168385.0, 0.0004], [167580.0, 0.001], [165580.0, 0.0005], [165410.0, 0.0005], [164755.0, 0.0001], [164660.0, 0.0035], [163580.0, 0.0005], [161410.0, 0.0006], [161200.0, 0.0031], [161110.0, 0.0038], [160665.0, 0.0001], [160580.0, 0.0005], [160000.0, 0.0313], [158635.0, 130.0812], [156965.0, 0.0216], [156920.0, 0.0036], [154510.0, 0.0003], [154500.0, 0.0032], [152110.0, 0.0038], [150000.0, 0.0117], [149180.0, 0.0126], [146500.0, 0.0024], [142035.0, 0.0054], [140000.0, 0.0035], [17600.0, 0.1136], [17400.0, 0.2298], [185.0, 0.082]], \"asks\": [[197490.0, 0.2056], [197495.0, 0.2643], [197500.0, 0.8831], [197505.0, 0.011], [197545.0, 1.7548], [197735.0, 0.009], [198000.0, 0.01], [198005.0, 0.6497], [198200.0, 0.5556], [198420.0, 0.575], [198485.0, 0.653], [198505.0, 0.6791], [198625.0, 0.015], [199000.0, 0.6638], [199310.0, 1.1022], [200010.0, 55.9356], [200050.0, 0.9599], [200200.0, 1.2584], [200300.0, 1.0629], [202330.0, 0.0025], [209970.0, 91.68], [218670.0, 0.0007], [220000.0, 0.0009], [228790.0, 102.9165], [229530.0, 110.9495], [230000.0, 0.005], [231340.0, 0.0021], [233550.0, 0.0005], [234915.0, 0.0002], [235310.0, 0.001], [237775.0, 0.0002], [239775.0, 0.0002], [240000.0, 0.001], [247420.0, 0.0002], [248235.0, 125.2265], [250295.0, 139.8331], [252275.0, 0.0146], [253655.0, 0.015], [254170.0, 0.001], [256150.0, 0.015], [256990.0, 0.015], [259670.0, 0.0001], [269250.0, 0.0006], [269670.0, 0.0001], [269675.0, 0.0002], [271870.0, 0.015], [272215.0, 0.015], [273575.0, 0.015], [275140.0, 0.015], [275460.0, 0.015], [275745.0, 0.015], [279670.0, 0.0001], [280920.0, 0.0146], [280945.0, 0.015], [281310.0, 0.015], [281355.0, 0.015], [281435.0, 0.015], [281535.0, 0.015], [281910.0, 0.015], [281925.0, 0.015], [282840.0, 0.015], [288650.0, 0.015], [289670.0, 0.0001], [290945.0, 0.015], [291300.0, 0.0009], [299670.0, 0.0001], [300000.0, 0.0113], [309295.0, 0.0002], [312455.0, 0.0007], [329755.0, 0.0007]], \"action\": \"partial\"}}";
            var lenght = 3233;
            var payloadAsByteArray = Encoding.ASCII.GetBytes(payload);
            var payloadResponse = JsonSerializer.Deserialize<WebSocketResponse>(payload);
            var orderbookResponse = JsonSerializer.Deserialize<OrderbookResponse>(payload);
            webSocketClient.Setup(o => o.ReceiveAsync(cancellationToken))
                           .Returns(Task.FromResult(new WebSocketClientResponse()
                           {
                               Message = payloadAsByteArray,
                               MessageType = WebSocketMessageType.Text,
                               Lenght = lenght,
                           }));
            dynamic result = new object();
            void PublishEvent<T>(T content) where T : class
            {
                result = content;
            }
            await exchange.ReceiveAsync(PublishEvent, cancellationToken);

            //Update

            //Assert
            Assert.IsType<OrderBook>(result);
            OrderBook teste = result as OrderBook;
            Assert.Equal(orderbookResponse.Data.Asks.Count(), teste.Asks.ToList().Count);
            Assert.Equal(orderbookResponse.Data.Bids.Count(), teste.Bids.ToList().Count);
        }
    }
}
