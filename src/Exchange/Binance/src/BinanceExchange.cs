using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Websockets;
using Crypto.Websocket.Extensions.Core.OrderBooks;
using Crypto.Websocket.Extensions.Core.OrderBooks.Models;
using Crypto.Websocket.Extensions.OrderBooks.Sources;
using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Binance
{
    public class BinanceExchange : IBinanceExchange, IExchange
    {
        private BinanceOrderBookSource BinanceOrderBookSource { get; }
        private BinanceWebsocketClient BinanceWebsocketClient { get; }
        private BinanceWebsocketCommunicator BinanceWebsocketCommunicator { get; }
        private List<SubscriptionBase> Subscriptions { get; }
        private IList<CryptoOrderBook> CryptoOrderBooks { get; }
        public BinanceExchange()
        {
            BinanceWebsocketCommunicator = new BinanceWebsocketCommunicator(Info.Uris.WebSocket.Main) { Name = Info.Name.DisplayName };
            BinanceWebsocketClient = new BinanceWebsocketClient(BinanceWebsocketCommunicator);
            BinanceOrderBookSource = new BinanceOrderBookSource(BinanceWebsocketClient);
            CryptoOrderBooks = new List<CryptoOrderBook>();
            Subscriptions = new List<SubscriptionBase>();
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
                    Name = ExchangeName.MercadoBitcoin,
                    Assets = new List<Asset>
                    {
                        Asset.BRL,
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
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.BRL, 50, 200000).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.BRL, 50, 200000).AddFee(decimal.Zero, 1.99m / 100m),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.BTC, 5 / 10m, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.BTC, 1 / 1m, 10).AddFee(4 / 10m, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.BCH, 1 / 10m, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.BCH, 1 / 1m, 25).AddFee(1 / 1m, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.LTC, 1 / 10m, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.LTC, 1 / 1m, 500).AddFee(1 / 1m, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.XRP, 0, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.XRP, 20, 20000).AddFee(1 / 100, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.ETH, 10 / 1m, decimal.MaxValue),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.ETH, 1 / 1m, 70),
                        new OperationInfo(OperationInfo.Types.Maker, decimal.MinValue, decimal.MaxValue).AddFee(decimal.Zero, 0.3m / 100m),
                        new OperationInfo(OperationInfo.Types.Taker, decimal.MinValue, decimal.MaxValue).AddFee(decimal.Zero, 0.7m / 100m),
                    },
                    Markets = new List<Market>
                    {
                        new Market(Asset.BTC, Asset.USDT),
                        new Market(Asset.ETH, Asset.USDT),
                        new Market(Asset.XRP, Asset.USDT),
                        new Market(Asset.LTC, Asset.USDT),
                    },
                    Country = Country.BRA,
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
                        WWW = new Uri("https://www.mercadobitcoin.com.br"),
                        Doc = new List<Uri>
                        {
                            new Uri("https://www.mercadobitcoin.com.br/api-doc/"),
                            new Uri("https://www.mercadobitcoin.com.br/trade-api/")
                        },
                        Fees = new List<Uri>
                        {
                            new Uri("https://www.mercadobitcoin.com.br/comissoes-prazos-limites")
                        },
                        WebApi = new WebApiUris
                        {
                            Public = new Uri("https://www.mercadobitcoin.net/api/"),
                            Private = new Uri("https://www.mercadobitcoin.net"),
                            Trade = new Uri("https://www.mercadobitcoin.net")
                        },
                        WebSocket = new WebSocketUris
                        {
                            Main = new Uri("wss://stream.binance.com:9443"),
                            Future = new Uri("wss://fstream.binance.com"),
                        },

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
                        HasWebSocket = true,
                        HasWebApi = false
                    }
                };
                return exchangeInfo;
            }
        }

        /// <summary>
        /// Exhange instance information information
        /// </summary>
        public ExchangeInfo Info => Information;

        #region OrderBook Public Methods

        public Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void SetOrderBookSubscription(Market market)
        {
            var pair = ToDataDomain(market);
            Subscriptions.Add(new OrderBookDiffSubscription(pair));
            CryptoOrderBooks.Add(new CryptoOrderBook(pair, BinanceOrderBookSource));
            _ = BinanceOrderBookSource.LoadSnapshot(BinanceWebsocketCommunicator, pair);
        }

        public void SubscribeToOrderBook(Action<IList<IOrderBookChangeInfo>> onNext)
        {
            BinanceWebsocketClient.SetSubscriptions(Subscriptions.ToArray());
            _ = BinanceWebsocketCommunicator.Start();
            Observable.CombineLatest(CryptoOrderBooks.Select(each => each.OrderBookUpdatedStream).ToArray()).Subscribe(onNext);
        }

        public Task SetOrderBookSubscription(List<Market> markets, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Subscribe(Action<OrderBook> onNext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AuthenticateAsync(ClientCredential clientCredentials)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeOrderbookAsync(Market market, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeOrderbookAsync(Market market, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReceiveAsync(Action<OrderBook> action, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task InitializeAsync(CancellationToken cancellationtoken)
        {
            throw new NotImplementedException();
        }

        public Task RestartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private static string ToDataDomain(Market market)
        {
            return $"{market.Base.DisplayName}{market.Quote.DisplayName}";
        }

        #endregion
    }
}
