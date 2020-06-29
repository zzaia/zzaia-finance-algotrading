using MagoTrader.Core.Exchange;
using MagoTrader.Core.Models;
using MagoTrader.Core.Services;
using MagoTrader.Exchange.MercadoBitcoin.Private;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Exchange.MercadoBitcoin.Trade;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public class MercadoBitcoinExchange : IMercadoBitcoinExchange, IExchange
    {
        private readonly PublicApiClient _publicApiClient;
        private readonly PrivateApiClient _privateApiClient;
        private readonly TradeApiClient _tradeApiClient;
        private readonly ILogger<MercadoBitcoinExchange> _logger;
        private readonly ClientCredential _tradeClientCredential;
        private readonly ClientCredential _privateClientCredential;

        private readonly string _apiResponseUnsuccessfully = "Public Api returned unsuccessfully, with message: ";

        /// <summary>
        /// Exchange client instance, selects api end-point based on parameter
        /// </summary>
        public MercadoBitcoinExchange(PublicApiClient publicApiClient,
                                      PrivateApiClient privateApiClient,
                                      TradeApiClient tradeApiClient,
                                      ILogger<MercadoBitcoinExchange> logger,
                                      IOptionsMonitor<ClientCredential> clientCredentials)
        {
            _tradeClientCredential = clientCredentials.Get(Information.Options.TradeClientCredentialReference);
            _privateClientCredential = clientCredentials.Get(Information.Options.PrivateClientCredentialReference);
            _publicApiClient = publicApiClient ?? throw new ArgumentNullException(nameof(publicApiClient));
            _privateApiClient = privateApiClient ?? throw new ArgumentNullException(nameof(privateApiClient));
            _tradeApiClient = tradeApiClient ?? throw new ArgumentNullException(nameof(tradeApiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _publicApiClient.SetBaseAddress(Information.Uris.Api.Public);
            _privateApiClient.SetBaseAddress(Information.Uris.Api.Private);
            _tradeApiClient.SetBaseAddress(Information.Uris.Api.Trade);
        }

        public IPublicApiClient PublicClient
        {
            get
            {
                return _publicApiClient;
            }
        }

        public IPrivateApiClient PrivateClient
        {
            get
            {
                return _privateApiClient;
            }
        }

        public ITradeApiClient TradeClient
        {
            get
            {
                return _tradeApiClient;
            }
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
                    Name = ExchangeNameEnum.MercadoBitcoin,
                    Fiats = new List<Asset>
                    {
                        new Asset("Real", AssetTickerEnum.BRL, 50, 200000, 0, 50, 200000, 1.99m / 100m),
                    },
                    Assets = new List<Asset>
                    {
                        new Asset("Bitcoin", AssetTickerEnum.BTC, 5 / 10m, decimal.MaxValue, 0, 1 / 1m, 10, 4 / 10m),
                        new Asset("Ethereum", AssetTickerEnum.ETH, 10 / 1m, decimal.MaxValue, 0, 1 / 1m, 70, 2 / 1m),
                        new Asset("Bitcoin Cash", AssetTickerEnum.BCH, 1 / 10m, decimal.MaxValue, 0, 1 / 1m, 25, 1 / 1m),
                        new Asset("Lite Coin", AssetTickerEnum.LTC, 1 / 10m, decimal.MaxValue, 0, 1 / 1m, 500, 1 / 1m),
                        new Asset("Ripple", AssetTickerEnum.XRP, 0, decimal.MaxValue, 0, 20, 20000, 1 / 100),
                    },
                    Markets = new List<Market>
                    {
                        new Market(AssetTickerEnum.BTC, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.BCH, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.ETH, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.LTC, AssetTickerEnum.BRL),
                        new Market(AssetTickerEnum.XRP, AssetTickerEnum.BRL),
                    },
                    Country = CountryEnum.BRA,
                    Culture = new CultureInfo("en-us"),
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
                        Api = new ApiUris
                        {
                            Public = new Uri("https://www.mercadobitcoin.net/api/"),
                            Private = new Uri("https://www.mercadobitcoin.net"),
                            Trade = new Uri("https://www.mercadobitcoin.net")
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
                    TradingFee = new MarketFee
                    {
                        TierBasedPercentage = false,
                        FixedPercentage = true,
                        Maker = 0.3m / 100m,
                        Taker = 0.7m / 100m,
                    },
                    Options = new ExchangeOptions
                    {
                        PrivateClientCredentialReference = Guid.NewGuid().ToString(),
                        TradeClientCredentialReference = Guid.NewGuid().ToString(),
                    }
                };
                return exchangeInfo;
            }
        }

        /// <summary>
        /// Exhange instance information information
        /// </summary>
        public ExchangeInfo Info { get { return Information; } }

        /// <summary>
        /// Fetch day summary OHLCV for the specified market and date.
        /// </summary>
        public async Task<ObjectResult<OHLCV>> FetchDaySummaryAsync(Market market, DateTimeOffset dateTime)
        {
            market = market ?? throw new ArgumentNullException(nameof(market));

            var response = await this.PublicClient.GetDaySummaryOHLCVAsync(market.Main.ToString(), dateTime.Year, dateTime.Month, dateTime.Day).ConfigureAwait(true);

            if (response.Success)
            {
                var resultToReturn = new OHLCV
                {
                    Exchange = ExchangeNameEnum.MercadoBitcoin,
                    TimeFrame = new TimeFrame(TimeFrameEnum.D1),
                    DateTimeOffset = DateTimeOffset.Parse(response.Output.Date, CultureInfo.InvariantCulture),
                    //DateTimeOffset = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day),
                    Market = market,
                    Open = response.Output.Open,
                    High = response.Output.High,
                    Low = response.Output.Low,
                    Close = response.Output.Close,
                    Volume = response.Output.Volume,
                    TradedQuantity = response.Output.TradedQuantity,
                    Average = response.Output.Average,
                    NumberOfTrades = response.Output.NumberOfTrades,
                };

                return ObjectResultFactory.CreateSuccessResult(resultToReturn);
            }
            else
            {
                _logger.LogError($"{_apiResponseUnsuccessfully}{response.ProblemDetails.Detail}");
                return ObjectResultFactory.CreateFailResult<OHLCV>();
            }
        }

        /// <summary>
        /// Fetch public orderbook for the specified market, returns a simplified orderbook by price.
        /// </summary>
        public async Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market)
        {
            market = market ?? throw new ArgumentNullException(nameof(market));

            var response = await this.PublicClient.GetOrderBookAsync(market.Main.ToString()).ConfigureAwait(true);

            if (response.Success)
            {
                var resultToReturn = new OrderBook
                {
                    Exchange = ExchangeNameEnum.MercadoBitcoin,
                    DateTimeOffset = DateTimeUtils.CurrentUtcDateTimeOffset(),
                    Market = market,
                    Bids = from order in response.Output.Bids.ToList()
                           select new Order(market, OrderTypeEnum.BUY, order[1], order[0]),
                    Asks = from order in response.Output.Bids.ToList()
                           select new Order(market, OrderTypeEnum.SELL, order[1], order[0]),
                };

                return ObjectResultFactory.CreateSuccessResult(resultToReturn);
            }
            else
            {
                _logger.LogError($"{_apiResponseUnsuccessfully}{response.ProblemDetails.Detail}");
                return ObjectResultFactory.CreateFailResult<OrderBook>();
            }
        }

        /// <summary>
        /// Fetch last 24h OHLCV for the specified market.
        /// </summary>
        public async Task<ObjectResult<OHLCV>> FetchOHLCVAsync(Market market)
        {
            market = market ?? throw new ArgumentNullException(nameof(market));

            var response = await this.PublicClient.GetLast24hOHLCVAsync(market.Main.ToString()).ConfigureAwait(true);

            if (response.Success)
            {
                var resultToReturn = new OHLCV
                {
                    Exchange = ExchangeNameEnum.MercadoBitcoin,
                    TimeFrame = new TimeFrame(TimeFrameEnum.D1),
                    DateTimeOffset = DateTimeUtils.TimestampToDateTimeOffset(response.Output.Ticker.TimeStamp, false),
                    Market = market,
                    Buy = Convert.ToDecimal(response.Output.Ticker.Buy, Information.Culture),
                    Sell = Convert.ToDecimal(response.Output.Ticker.Sell, Information.Culture),
                    High = Convert.ToDecimal(response.Output.Ticker.High, Information.Culture),
                    Low = Convert.ToDecimal(response.Output.Ticker.Low, Information.Culture),
                    Last = Convert.ToDecimal(response.Output.Ticker.Last, Information.Culture),
                    Volume = Convert.ToDecimal(response.Output.Ticker.Volume, Information.Culture),
                };

                return ObjectResultFactory.CreateSuccessResult(resultToReturn);
            }
            else
            {
                _logger.LogError($"{_apiResponseUnsuccessfully}{response.ProblemDetails.Detail}");
                return ObjectResultFactory.CreateFailResult<OHLCV>();
            }
        }

        /// <summary>
        /// Fetch list of trades for the specified market, return 1000 last.
        /// </summary>
        public async Task<ObjectResult<IEnumerable<Order>>> FetchTradesAsync(Market market)
        {
            market = market ?? throw new ArgumentNullException(nameof(market));

            var response = await this.PublicClient.GetLastTradesAsync(market.Main.ToString()).ConfigureAwait(true);

            if (response.Success)
            {
                IEnumerable<Order> resultToReturn
                    = from trade in response.Output
                      select new Order(market,
                                       OrderTypeEnum.Parse<OrderTypeEnum>(trade.Type),
                                       trade.Amount,
                                       trade.Price,
                                       new Guid(trade.Tid.GetHashCode().ToString(CultureInfo.InvariantCulture)),
                                       DateTimeUtils.TimestampToDateTimeOffset(trade.TimeStamp, false),
                                       OrderStatusEnum.CLOSED
                          );

                return ObjectResultFactory.CreateSuccessResult(resultToReturn);
            }
            else
            {
                _logger.LogError($"{_apiResponseUnsuccessfully} {response.ProblemDetails.Detail}");
                return ObjectResultFactory.CreateFailResult<IEnumerable<Order>>();
            }
        }
    }
}
