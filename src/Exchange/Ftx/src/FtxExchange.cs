using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.Exchange.Ftx.WebSockets;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx
{
    public class FtxExchange : IFtxExchange, IExchange
    {
        private readonly ILogger<FtxExchange> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly ClientCredential _tradeClientCredential;
        private readonly ClientCredential _privateClientCredential;
        private readonly FtxWebSocketClient _websocketClient;

        public FtxExchange(Action<ClientCredential> privateClientCredentials,
                           Action<ClientCredential> tradeClientCredentials,
                           ILogger<FtxExchange> logger,
                           TelemetryClient telemetryClient,
                           IHttpClientFactory clientFactory)
        {
            privateClientCredentials = privateClientCredentials ?? throw new ArgumentNullException(nameof(privateClientCredentials));
            var privateClientCredentialsModel = new ClientCredential();
            privateClientCredentials.Invoke(privateClientCredentialsModel);
            _tradeClientCredential = privateClientCredentialsModel;

            tradeClientCredentials = tradeClientCredentials ?? throw new ArgumentNullException(nameof(tradeClientCredentials));
            var tradeClientCredentialsModel = new ClientCredential();
            tradeClientCredentials.Invoke(tradeClientCredentialsModel);
            _privateClientCredential = tradeClientCredentialsModel;

            clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
            _websocketClient = new FtxWebSocketClient(Information.Uris.WebSocket.Main);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
        }

        /// <summary>
        /// Exhange static information
        /// </summary>
        public static ExchangeInfo Information
        {
            get
            {
                var exchangeInfo = new ExchangeInfo()
                {
                    Name = ExchangeName.Ftx,
                    Assets = new List<Asset>
                    {
                        Asset.BRZ,
                        Asset.BCH,
                        Asset.BTC,
                        Asset.ETH,
                        Asset.LTC,
                        Asset.XRP,
                        Asset.WBX,
                        Asset.CHZ,
                        Asset.USDC
                    },
                    Operations = new List<OperationInfo>
                    {
                    },
                    Markets = new List<Market>
                    {
                        new Market(Asset.BTC, Asset.BRZ),
                        new Market(Asset.ETH, Asset.BRZ),
                        new Market(Asset.USDC, Asset.BRZ),
                    },
                    Country = Country.USA,
                    Culture = new CultureInfo("en-us"),
                    Timeframes = new List<TimeFrame>
                    {
                        TimeFrame.m15,
                        TimeFrame.m30,
                        TimeFrame.H1,
                        TimeFrame.H2,
                        TimeFrame.H4,
                        TimeFrame.H6,
                        TimeFrame.H8,
                        TimeFrame.H12,
                        TimeFrame.D1,
                        TimeFrame.D3,
                        TimeFrame.W1,
                        TimeFrame.W2,
                    },
                    Uris = new ExchangeUris
                    {
                        WWW = new Uri(""),
                        Doc = new List<Uri>
                        {
                            new Uri(""),
                            new Uri("")
                        },
                        Fees = new List<Uri>
                        {
                            new Uri("")
                        },
                        WebApi = new WebApiUris
                        {
                            Public = new Uri(""),
                            Private = new Uri(""),
                            Trade = new Uri("")
                        },
                        WebSocket = new WebSocketUris
                        {
                            Main = new Uri("wss://ftx.com/ws/")
                        }
                    },
                    RequiredCredentials = new RequiredCredentials
                    {
                        Id = true,
                        Secret = true,
                        Login = false,
                        Password = false,
                        Twofa = false,
                    },
                    LimitRate = new ExchangeLimitRate
                    {
                        Rate = 1
                    },
                    Options = new ExchangeOptions
                    {
                        HasWebApi = true,
                        HasWebSocket = true
                    }
                };
                return exchangeInfo;
            }
        }

        /// <summary>
        /// Exhange instance information
        /// </summary>
        public ExchangeInfo Info => Information;


        public Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SetOrderBookSubscription(Market market, CancellationToken cancellationToken)
        {
            await _websocketClient.SubscribeOrderbook(market.Ticker, cancellationToken);
        }

        public async Task SubscribeToOrderBook(Action<OrderBook> onNext, CancellationToken cancellationToken)
        {
            await _websocketClient.ConnectAsync(cancellationToken);
            var response = await _websocketClient.ReceiveAsync(cancellationToken);
            if (response.MessageType == WebSocketMessageType.Text)
            {
                string responseMessage = Encoding.UTF8.GetString(response.Message, 0, response.Lenght);
                //await ProcessMessage(responseMessage, cancellationToken);
                _logger.LogDebug(responseMessage);
            }
        }
    }
}
