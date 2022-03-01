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
        public FtxExchangeShould()
        {

        }

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
            var payload = "{\"channel\": \"orderbook\", \"market\": \"ETH/BRZ\", \"type\": \"partial\", \"data\": {\"time\": 1646003954.8245392, \"checksum\": 654482407, \"bids\": [[13528.0, 17.3931], [13520.0, 0.2653], [13519.0, 0.02], [13518.0, 17.4842], [13505.0, 5.4159], [13493.0, 0.03], [13483.0, 0.0393], [13482.0, 8.5375], [13469.0, 8.7456], [13455.0, 9.0711], [13442.0, 12.2055], [13426.0, 13.8254], [13420.0, 0.0004], [13400.0, 0.0004], [13380.0, 0.0004], [13376.0, 14.4987], [13360.0, 0.0004], [13340.0, 0.0004], [13320.0, 0.0004], [13315.0, 10.5032], [13303.0, 21.6563], [13300.0, 0.0002], [13288.0, 17.2436], [13280.0, 0.0002], [13260.0, 0.0002], [13240.0, 0.0002], [13220.0, 0.0002], [13200.0, 0.0002], [13180.0, 0.0002], [13160.0, 0.0002], [13140.0, 0.0002], [13120.0, 0.0002], [13100.0, 0.0002], [13080.0, 0.0002], [13060.0, 0.0002], [13040.0, 0.0002], [13020.0, 0.0002], [13000.0, 0.0002], [12980.0, 0.0002], [12960.0, 0.0002], [12940.0, 0.0002], [12920.0, 0.0002], [12900.0, 0.0002], [12880.0, 0.0002], [12860.0, 0.0002], [12840.0, 0.0002], [12820.0, 0.0002], [12800.0, 0.008], [12792.0, 0.209], [12780.0, 0.0002], [12760.0, 0.0002], [12740.0, 0.0002], [12720.0, 0.0002], [12700.0, 0.0002], [12637.0, 753.1944], [12377.0, 0.025], [12235.0, 0.0408], [12212.0, 0.0818], [12150.0, 0.0261], [12100.0, 1189.2636], [12000.0, 0.0666], [11999.0, 0.0416], [11977.0, 0.0793], [11600.0, 0.0093], [11500.0, 0.0065], [11144.0, 1705.3004], [11092.0, 1612.7954], [11009.0, 0.0011], [11000.0, 0.1979], [10692.0, 1769.4395], [10229.0, 2039.0729], [10000.0, 1.001], [9100.0, 0.0032], [9000.0, 0.6106], [8248.0, 0.1494], [5112.0, 0.0058], [1112.0, 0.0269]], \"asks\": [[13615.0, 0.1448], [13616.0, 0.0004], [13628.0, 14.2925], [13636.0, 0.0004], [13642.0, 4.6157], [13656.0, 0.0004], [13669.0, 6.9007], [13676.0, 0.0004], [13693.0, 9.1516], [13696.0, 0.0004], [13702.0, 10.223], [13716.0, 0.0004], [13736.0, 0.0004], [13742.0, 10.1815], [13752.0, 10.8134], [13756.0, 0.0004], [13767.0, 14.0469], [13776.0, 0.0004], [13784.0, 11.2088], [13796.0, 0.0004], [13816.0, 0.0004], [13832.0, 12.0804], [13835.0, 12.4105], [13836.0, 0.0004], [13856.0, 0.0004], [13864.0, 16.9517], [13876.0, 0.0004], [13896.0, 0.0004], [13920.0, 0.0004], [13936.0, 0.0004], [13960.0, 0.0004], [13980.0, 0.0004], [14000.0, 0.0004], [14020.0, 0.0004], [14040.0, 0.0004], [14054.0, 0.0373], [14060.0, 0.0004], [14080.0, 0.0004], [14100.0, 0.0004], [14120.0, 0.0004], [14140.0, 0.0004], [14160.0, 0.0004], [14180.0, 0.0004], [14200.0, 0.0004], [14220.0, 0.0004], [14240.0, 0.0004], [14258.0, 0.0001], [14260.0, 0.0004], [14280.0, 0.0004], [14304.0, 0.0004], [14324.0, 0.0004], [14344.0, 0.0004], [14364.0, 0.0004], [14384.0, 0.0004], [14404.0, 0.0004], [14410.0, 855.9261], [14424.0, 0.0004], [14444.0, 0.0004], [14464.0, 0.0004], [14484.0, 0.0004], [14504.0, 0.0004], [14516.0, 0.0001], [14544.0, 0.0004], [14564.0, 0.0004], [14584.0, 0.0004], [14604.0, 0.0004], [14624.0, 0.0004], [14644.0, 0.0004], [14664.0, 0.0004], [14684.0, 0.0004], [14704.0, 0.0004], [14728.0, 0.0004], [14748.0, 0.0004], [14768.0, 0.0004], [14774.0, 0.0001], [14788.0, 0.0004], [14808.0, 0.0004], [14828.0, 0.0004], [14848.0, 0.0004], [14868.0, 0.0004], [14888.0, 0.0004], [14916.0, 0.001], [14936.0, 0.001], [14956.0, 0.001], [14976.0, 0.001], [14996.0, 0.001], [15000.0, 0.0039], [15016.0, 0.001], [15032.0, 0.0001], [15036.0, 0.001], [15056.0, 0.001], [15076.0, 0.001], [15096.0, 0.001], [15116.0, 0.001], [15136.0, 0.001], [15156.0, 0.001], [15176.0, 0.001], [15196.0, 0.001], [15216.0, 0.001], [15236.0, 0.001]], \"action\": \"partial\"}}";
            var lenght = 3534;
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
            var payload = "{\"channel\": \"orderbook\", \"market\": \"ETH/BRZ\", \"type\": \"partial\", \"data\": {\"time\": 1646003954.8245392, \"checksum\": 654482407, \"bids\": [[13528.0, 17.3931], [13520.0, 0.2653], [13519.0, 0.02], [13518.0, 17.4842], [13505.0, 5.4159], [13493.0, 0.03], [13483.0, 0.0393], [13482.0, 8.5375], [13469.0, 8.7456], [13455.0, 9.0711], [13442.0, 12.2055], [13426.0, 13.8254], [13420.0, 0.0004], [13400.0, 0.0004], [13380.0, 0.0004], [13376.0, 14.4987], [13360.0, 0.0004], [13340.0, 0.0004], [13320.0, 0.0004], [13315.0, 10.5032], [13303.0, 21.6563], [13300.0, 0.0002], [13288.0, 17.2436], [13280.0, 0.0002], [13260.0, 0.0002], [13240.0, 0.0002], [13220.0, 0.0002], [13200.0, 0.0002], [13180.0, 0.0002], [13160.0, 0.0002], [13140.0, 0.0002], [13120.0, 0.0002], [13100.0, 0.0002], [13080.0, 0.0002], [13060.0, 0.0002], [13040.0, 0.0002], [13020.0, 0.0002], [13000.0, 0.0002], [12980.0, 0.0002], [12960.0, 0.0002], [12940.0, 0.0002], [12920.0, 0.0002], [12900.0, 0.0002], [12880.0, 0.0002], [12860.0, 0.0002], [12840.0, 0.0002], [12820.0, 0.0002], [12800.0, 0.008], [12792.0, 0.209], [12780.0, 0.0002], [12760.0, 0.0002], [12740.0, 0.0002], [12720.0, 0.0002], [12700.0, 0.0002], [12637.0, 753.1944], [12377.0, 0.025], [12235.0, 0.0408], [12212.0, 0.0818], [12150.0, 0.0261], [12100.0, 1189.2636], [12000.0, 0.0666], [11999.0, 0.0416], [11977.0, 0.0793], [11600.0, 0.0093], [11500.0, 0.0065], [11144.0, 1705.3004], [11092.0, 1612.7954], [11009.0, 0.0011], [11000.0, 0.1979], [10692.0, 1769.4395], [10229.0, 2039.0729], [10000.0, 1.001], [9100.0, 0.0032], [9000.0, 0.6106], [8248.0, 0.1494], [5112.0, 0.0058], [1112.0, 0.0269]], \"asks\": [[13615.0, 0.1448], [13616.0, 0.0004], [13628.0, 14.2925], [13636.0, 0.0004], [13642.0, 4.6157], [13656.0, 0.0004], [13669.0, 6.9007], [13676.0, 0.0004], [13693.0, 9.1516], [13696.0, 0.0004], [13702.0, 10.223], [13716.0, 0.0004], [13736.0, 0.0004], [13742.0, 10.1815], [13752.0, 10.8134], [13756.0, 0.0004], [13767.0, 14.0469], [13776.0, 0.0004], [13784.0, 11.2088], [13796.0, 0.0004], [13816.0, 0.0004], [13832.0, 12.0804], [13835.0, 12.4105], [13836.0, 0.0004], [13856.0, 0.0004], [13864.0, 16.9517], [13876.0, 0.0004], [13896.0, 0.0004], [13920.0, 0.0004], [13936.0, 0.0004], [13960.0, 0.0004], [13980.0, 0.0004], [14000.0, 0.0004], [14020.0, 0.0004], [14040.0, 0.0004], [14054.0, 0.0373], [14060.0, 0.0004], [14080.0, 0.0004], [14100.0, 0.0004], [14120.0, 0.0004], [14140.0, 0.0004], [14160.0, 0.0004], [14180.0, 0.0004], [14200.0, 0.0004], [14220.0, 0.0004], [14240.0, 0.0004], [14258.0, 0.0001], [14260.0, 0.0004], [14280.0, 0.0004], [14304.0, 0.0004], [14324.0, 0.0004], [14344.0, 0.0004], [14364.0, 0.0004], [14384.0, 0.0004], [14404.0, 0.0004], [14410.0, 855.9261], [14424.0, 0.0004], [14444.0, 0.0004], [14464.0, 0.0004], [14484.0, 0.0004], [14504.0, 0.0004], [14516.0, 0.0001], [14544.0, 0.0004], [14564.0, 0.0004], [14584.0, 0.0004], [14604.0, 0.0004], [14624.0, 0.0004], [14644.0, 0.0004], [14664.0, 0.0004], [14684.0, 0.0004], [14704.0, 0.0004], [14728.0, 0.0004], [14748.0, 0.0004], [14768.0, 0.0004], [14774.0, 0.0001], [14788.0, 0.0004], [14808.0, 0.0004], [14828.0, 0.0004], [14848.0, 0.0004], [14868.0, 0.0004], [14888.0, 0.0004], [14916.0, 0.001], [14936.0, 0.001], [14956.0, 0.001], [14976.0, 0.001], [14996.0, 0.001], [15000.0, 0.0039], [15016.0, 0.001], [15032.0, 0.0001], [15036.0, 0.001], [15056.0, 0.001], [15076.0, 0.001], [15096.0, 0.001], [15116.0, 0.001], [15136.0, 0.001], [15156.0, 0.001], [15176.0, 0.001], [15196.0, 0.001], [15216.0, 0.001], [15236.0, 0.001]], \"action\": \"partial\"}}";
            var lenght = 3534;
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
            payload = "{\"channel\": \"orderbook\", \"market\": \"ETH/BRZ\", \"type\": \"update\", \"data\": {\"time\": 1646003954.8791366, \"checksum\": 30530270, \"bids\": [], \"asks\": [[13615.0, 0.0], [15256.0, 0.001]], \"action\": \"update\"}}";
            lenght = 199;
            payloadAsByteArray = Encoding.ASCII.GetBytes(payload);
            webSocketClient.Setup(o => o.ReceiveAsync(cancellationToken))
                           .Returns(Task.FromResult(new WebSocketClientResponse()
                           {
                               Message = payloadAsByteArray,
                               MessageType = WebSocketMessageType.Text,
                               Lenght = lenght,
                           }));
            await exchange.ReceiveAsync(PublishEvent, cancellationToken);

            //Assert
            Assert.IsType<OrderBook>(result);
            OrderBook teste = result as OrderBook;
            Assert.Equal(orderbookResponse.Data.Asks.Count(), teste.Asks.ToList().Count);
            Assert.Equal(orderbookResponse.Data.Bids.Count(), teste.Bids.ToList().Count);
        }
    }
}
