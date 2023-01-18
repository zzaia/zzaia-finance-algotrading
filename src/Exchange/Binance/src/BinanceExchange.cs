using Zzaia.Finance.Core.Interfaces.ExchangeAggregate;
using Zzaia.Finance.Core.Models;
using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.ExchangeAggregate;
using Zzaia.Finance.Core.Models.MarketAgregate;
using Zzaia.Finance.Core.Models.OrderBookAggregate;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Zzaia.Finance.Exchange.Binance
{
    public class BinanceExchange : IBinanceExchange, IExchange
    {
        public BinanceExchange()
        {
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
                    Name = ExchangeName.Binance,
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
                    },
                    Markets = new List<Market>
                    {
                        new Market(Asset.BTC, Asset.USDT),
                        new Market(Asset.ETH, Asset.USDT),
                        new Market(Asset.XRP, Asset.USDT),
                        new Market(Asset.LTC, Asset.USDT),
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

        public Task ReceiveAsync(Action<dynamic> action, CancellationToken cancellationToken)
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

        public Task ConfirmLivenessAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
