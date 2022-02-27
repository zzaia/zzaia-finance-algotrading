using Force.Crc32;
using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.Exchange.Ftx.WebSocket.Models;
using MarketIntelligency.Exchange.Ftx.WebSockets.Models;
using MarketIntelligency.WebSocket;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx
{
    public class FtxExchange : IFtxExchange, IExchange
    {
        private readonly ILogger<FtxExchange> _logger;
        private readonly IWebSocketClient _websocketClient;
        private readonly TelemetryClient _telemetryClient;
        private readonly ClientCredential _tradeClientCredential;
        private readonly ClientCredential _privateClientCredential;

        private List<WebSocketRequest> _subscriptions;
        private List<OrderBook> _snapShots;
        public FtxExchange(Action<ClientCredential> privateClientCredentials,
                           Action<ClientCredential> tradeClientCredentials,
                           ILogger<FtxExchange> logger,
                           TelemetryClient telemetryClient,
                           IHttpClientFactory clientFactory,
                           IWebSocketClient webSocketClient)
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
            _websocketClient = webSocketClient ?? throw new ArgumentNullException(nameof(webSocketClient));
            _websocketClient.SetBaseAddress(Information.Uris.WebSocket.Main.AbsoluteUri);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
            _subscriptions = new List<WebSocketRequest>();
            _snapShots = new List<OrderBook>();
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
                        HasWebSocket = true,
                        CheckForLivenessTimeSpan = TimeSpan.FromSeconds(15),
                    }
                };
                return exchangeInfo;
            }
        }

        /// <summary>
        /// Exhange instance information
        /// </summary>
        public ExchangeInfo Info => Information;


        #region Public Methods

        public Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task SubscribeOrderbookAsync(Market market, CancellationToken stoppingToken)
        {
            try
            {
                var subscribeRequest = new WebSocketRequest(WebSocketRequest.ChannelTypes.Orderbook, market.Ticker, WebSocketRequest.OperationTypes.Subscribe);
                var subscribeRequestMessage = JsonSerializer.Serialize(subscribeRequest);
                await _websocketClient.SendTextAsync(subscribeRequestMessage, stoppingToken);
                _subscriptions.Add(subscribeRequest);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task UnsubscribeOrderbookAsync(Market market, CancellationToken stoppingToken)
        {
            try
            {
                var unsubscribeRequest = new WebSocketRequest(WebSocketRequest.ChannelTypes.Orderbook, market.Ticker, WebSocketRequest.OperationTypes.Unsubscribe);
                var unsubscribeRequestMessage = JsonSerializer.Serialize(unsubscribeRequest);
                await _websocketClient.SendTextAsync(unsubscribeRequestMessage, stoppingToken);
                _subscriptions.Remove(unsubscribeRequest);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task ConfirmLivenessAsync(CancellationToken stoppingToken)
        {
            try
            {
                var unsubscribeRequest = new WebSocketRequest(WebSocketRequest.ChannelTypes.Orderbook, string.Empty, WebSocketRequest.OperationTypes.Ping);
                var unsubscribeRequestMessage = JsonSerializer.Serialize(unsubscribeRequest);
                await _websocketClient.SendTextAsync(unsubscribeRequestMessage, stoppingToken);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public Task AuthenticateAsync(ClientCredential clientCredentials)
        {
            throw new NotImplementedException();
        }

        public async Task InitializeAsync(CancellationToken cancellationtoken)
        {
            try
            {
                await _websocketClient.ConnectAsync(cancellationtoken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RestartAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _websocketClient.ReconnectAsync(cancellationToken);
                foreach (var subscription in _subscriptions)
                {
                    var unsubscribeRequestMessage = JsonSerializer.Serialize(subscription);
                    await _websocketClient.SendTextAsync(unsubscribeRequestMessage, cancellationToken);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task ReceiveAsync(Action<dynamic> action, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _websocketClient.ReceiveAsync(cancellationToken);
                if (response.MessageType == WebSocketMessageType.Text)
                {
                    string responseMessage = Encoding.UTF8.GetString(response.Message, 0, response.Lenght);
                    Console.WriteLine(responseMessage);
                    Console.WriteLine(responseMessage.Length);
                    var payloadResponse = JsonSerializer.Deserialize<WebSocketResponse>(responseMessage);
                    if (payloadResponse != null && payloadResponse.Channel != null && payloadResponse.Type != null)
                    {
                        if (payloadResponse.Channel.Equals(WebSocketRequest.ChannelTypes.Orderbook))
                        {
                            if (payloadResponse.Type.Equals(WebSocketResponse.Types.Partial))
                            {
                                var orderbookResponse = JsonSerializer.Deserialize<OrderbookResponse>(responseMessage);
                                var milliseconds = orderbookResponse.Data.Time % 10;
                                var ticks = (long)(milliseconds * TimeSpan.TicksPerMillisecond);
                                var seconds = (long)(orderbookResponse.Data.Time - milliseconds);
                                var snapShot = new OrderBook()
                                {
                                    Exchange = Info.Name,
                                    DateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(seconds).AddTicks(ticks),
                                    Market = new Market(payloadResponse.Market),
                                    Asks = orderbookResponse.Data.Asks.Select(each => new OrderBookLevel(each[0], each[1])),
                                    Bids = orderbookResponse.Data.Bids.Select(each => new OrderBookLevel(each[0], each[1])),
                                };
                                var oldSnapshot = _snapShots.SingleOrDefault(one => one.Market.Ticker.Equals(snapShot.Market.Ticker));
                                if (oldSnapshot != null) _snapShots.Remove(oldSnapshot);
                                _snapShots.Add(snapShot);
                                //action.Invoke(snapShot);
                            }
                            else if (payloadResponse.Type.Equals(WebSocketResponse.Types.Update))
                            {
                                var orderbookResponse = JsonSerializer.Deserialize<OrderbookResponse>(responseMessage);
                                var milliseconds = orderbookResponse.Data.Time % 10;
                                var ticks = (long)(milliseconds * TimeSpan.TicksPerMillisecond);
                                var seconds = (long)(orderbookResponse.Data.Time - milliseconds);
                                var oldSnapshot = _snapShots.Single(one => one.Market.Ticker.Equals(new Market(payloadResponse.Market).Ticker));
                                oldSnapshot.DateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(seconds).AddTicks(ticks);
                                foreach (var item in orderbookResponse.Data.Asks)
                                {
                                    var orderbookLevelToUpdate = oldSnapshot.Asks.Where(one => one.Price.Equals(item[0])).SingleOrDefault();
                                    if (orderbookLevelToUpdate != null)
                                    {
                                        if (item[1] == decimal.Zero)
                                        {
                                            oldSnapshot.Asks = oldSnapshot.Asks.Where((one) => !one.Price.Equals(item[0])).ToList();
                                        }
                                        else
                                        {
                                            orderbookLevelToUpdate.Amount = item[1];
                                        }
                                    }
                                    else
                                    {
                                        var firstCollection = oldSnapshot.Asks.Where(one => one.Price < item[0]).ToList();
                                        firstCollection.Add(new OrderBookLevel(item[0], item[1]));
                                        var secondCollection = oldSnapshot.Asks.Where(one => one.Price > item[0]).ToList();
                                        firstCollection.AddRange(secondCollection);
                                        oldSnapshot.Asks = firstCollection;
                                    }
                                }
                                foreach (var item in orderbookResponse.Data.Bids)
                                {
                                    var orderbookLevelToUpdate = oldSnapshot.Bids.Where(one => one.Price.Equals(item[0])).SingleOrDefault();
                                    if (orderbookLevelToUpdate != null)
                                    {
                                        if (item[1] == decimal.Zero)
                                        {
                                            oldSnapshot.Bids = oldSnapshot.Bids.Where((one) => !one.Price.Equals(item[0])).ToList();
                                        }
                                        else
                                        {
                                            orderbookLevelToUpdate.Amount = item[1];
                                        }
                                    }
                                    else
                                    {
                                        var firstCollection = oldSnapshot.Bids.Where(one => one.Price > item[0]).ToList();
                                        firstCollection.Add(new OrderBookLevel(item[0], item[1]));
                                        var secondCollection = oldSnapshot.Bids.Where(one => one.Price < item[0]).ToList();
                                        firstCollection.AddRange(secondCollection);
                                        oldSnapshot.Bids = firstCollection;
                                    }
                                }

                                var checksumString = string.Empty;
                                var asksCount = oldSnapshot.Asks.Count();
                                var bidsCount = oldSnapshot.Bids.Count();
                                var maxIteration = asksCount > bidsCount ? asksCount : bidsCount;
                                var bidsArray = oldSnapshot.Bids.ToArray();
                                var asksArray = oldSnapshot.Asks.ToArray();
                                for (int i = 0; i < maxIteration; i++)
                                {
                                    if (i != 0)
                                    {
                                        checksumString += ":";
                                    }

                                    if (i < bidsCount)
                                    {
                                        checksumString += $"{bidsArray[i].Price}:{bidsArray[i].Amount}";
                                    }

                                    if (i < asksCount)
                                    {
                                        if (checksumString.Last() != ':') checksumString += ":";
                                        checksumString += $"{asksArray[i].Price}:{asksArray[i].Amount}";
                                    }
                                }
                                var bytes = Encoding.ASCII.GetBytes(checksumString);
                                var crc32 = new Crc32Algorithm().ComputeHash(bytes);
                                var checkSumToConfirm = BitConverter.ToInt32(crc32, 0);
                                if (checkSumToConfirm == orderbookResponse.Data.Checksum)
                                {
                                    //action.Invoke(oldSnapshot);
                                }
                                else
                                {
                                    //await RestartAsync(cancellationToken);
                                }
                            }
                        }
                        else if (payloadResponse.Channel.Equals(WebSocketRequest.ChannelTypes.Trades))
                        {

                        }
                        else if (payloadResponse.Channel.Equals(WebSocketRequest.ChannelTypes.Ticker))
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion
    }
}
