using System;
using System.Collections.Generic;
using System.Text;

using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Exchange.MercadoBitcoin.Private;
using MagoTrader.Exchange.MercadoBitcoin.Trade;
using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class MercadoBitcoin : IExchange
    {
        private readonly PublicApiClient _publicApiClient;
        private readonly PrivateApiClient _privateApiClient;
        private readonly TradeApiClient _tradeApiClient;

        /// <summary>
        /// Exchange client instance, selects api end-point based on parameter
        /// </summary>
        public MercadoBitcoin(  PublicApiClient publicApiClient,
                                PrivateApiClient privateApiClient,
                                TradeApiClient tradeApiClient)
        {
            _publicApiClient = publicApiClient ?? throw new ArgumentNullException(nameof(publicApiClient));
            _privateApiClient = privateApiClient ?? throw new ArgumentNullException(nameof(privateApiClient));
            _tradeApiClient = tradeApiClient ?? throw new ArgumentNullException(nameof(tradeApiClient));
        }
        public IPublicApiClient Public
        {
            get
            {
                return _publicApiClient;
            }
        }
        
        public IPrivateApiClient Private
        {
            get
            {
                return _privateApiClient;
            }
        }

        public ITradeApiClient Trade
        {
            get
            {
                return _tradeApiClient;
            }
        }

        /// <summary>
        /// Exhange information
        /// </summary>
        public ExchangeInfo Info
        {
            get
            {
                var exchangeInfo = new ExchangeInfo()
                {
                    Name = ExchangeNameEnum.MercadoBitcoin,
                    Fiats = new List<Asset>
                    {
                        new Asset("Real", AssetTickerEnum.BRL, 50, 200000, 0, 50, 200000, 1.99m/100m ),
                    },
                    Assets = new List<Asset> 
                    {
                        new Asset("Bitcoin", AssetTickerEnum.BTC, 5/10m, decimal.MaxValue, 0, 1/1m, 10, 4/10m ),
                        new Asset("Ethereum", AssetTickerEnum.ETH, 10/1m, decimal.MaxValue, 0, 1/1m, 70, 2/1m ),
                        new Asset("Bitcoin Cash", AssetTickerEnum.BCH, 1/10m, decimal.MaxValue, 0, 1/1m, 25, 1/1m ),
                        new Asset("Lite Coin", AssetTickerEnum.LTC, 1/10m, decimal.MaxValue, 0, 1/1m, 500, 1/1m ),
                        new Asset("Ripple", AssetTickerEnum.XRP, 0, decimal.MaxValue, 0, 20, 20000, 1/100 ),
                    },
                    Markets = new List<Market>
                    {
                        new Market(AssetTickerEnum.BCH, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.BCH, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.ETH, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.LTC, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.XRP, AssetTickerEnum.BRL),
                    },
                    Countries = new List<CountryEnum>
                    {
                        CountryEnum.BRA
                    },
                    Timeframes = new List<TimeFrame>
                    {
                        new TimeFrame(TimeFrameEnum.m15),
                        new TimeFrame(TimeFrameEnum.m30),
                        new TimeFrame(TimeFrameEnum.H1),
                        new TimeFrame(TimeFrameEnum.H2),
                        new TimeFrame(TimeFrameEnum.H4),
                        new TimeFrame(TimeFrameEnum.H6),
                        new TimeFrame(TimeFrameEnum.H8),
                        new TimeFrame(TimeFrameEnum.H12),
                        new TimeFrame(TimeFrameEnum.D1),
                        new TimeFrame(TimeFrameEnum.D3),
                        new TimeFrame(TimeFrameEnum.W1),
                        new TimeFrame(TimeFrameEnum.W2),
                    },
                    Urls = new ExchangeUrls
                    {
                        WWW = "https://www.mercadobitcoin.com.br",
                        Doc = new List<string>
                        {
                            "https://www.mercadobitcoin.com.br/api-doc/",
                            "https://www.mercadobitcoin.com.br/trade-api/"
                        },
                        Fees = new List<string>
                        {
                            "https://www.mercadobitcoin.com.br/comissoes-prazos-limites"
                        },
                        Api = new ApiUrls
                        {
                            Private = "https://www.mercadobitcoin.net/api",
                            Public = "https://www.mercadobitcoin.net/tapi/v3",
                            Trade = "https://www.mercadobitcoin.net/tapi/v3"
                        },
                    },
                    RequiredCredentials = new RequiredCredentials
                    {
                        Apikey = true,
                        Secret = true,
                        Login = false,
                        Password = false,
                        Twofa = false,
                        Uid = false,
                    },
                    TradingFee = new MarketFee
                    {
                        TierBasedPercentage = false,
                        FixedPercentage = true,
                        Maker = 0.3m / 100m,
                        Taker = 0.7m / 100m,
                    }
                };
                return exchangeInfo;
            }
        }
    }
}
