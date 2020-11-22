using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Utils;
using MarketIntelligency.Exchange.MercadoBitcoin.Private;
using MarketIntelligency.Exchange.MercadoBitcoin.Public;
using MarketIntelligency.Exchange.MercadoBitcoin.Trade;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin
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
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.BRL,50, 200000).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.BRL, 50, 200000).AddFee(decimal.Zero, 1.99m / 100m),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.BTC, 5 / 10m, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.BTC, 1 / 1m, 10).AddFee( 4 / 10m, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.BCH, 1 / 10m, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.BCH, 1 / 1m, 25).AddFee( 1 / 1m, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.LTC, 1 / 10m, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.LTC, 1 / 1m, 500).AddFee( 1 / 1m, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.XRP, 0, decimal.MaxValue).AddFee(decimal.Zero, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.XRP, 20, 20000).AddFee( 1 / 100, decimal.Zero),
                        new OperationInfo(OperationInfo.Types.Deposit, Asset.ETH, 10 / 1m, decimal.MaxValue),
                        new OperationInfo(OperationInfo.Types.Withdrawal, Asset.ETH, 1 / 1m, 70),
                        new OperationInfo(OperationInfo.Types.Maker, decimal.MinValue, decimal.MaxValue).AddFee(decimal.Zero, 0.3m / 100m),
                        new OperationInfo(OperationInfo.Types.Taker, decimal.MinValue, decimal.MaxValue).AddFee(decimal.Zero, 0.7m / 100m),
                    },
                    Markets = new List<Market>
                    {
                        new Market(Asset.BTC, Asset.BRL),
                        new Market(Asset.BCH, Asset.BRL),
                        new Market(Asset.ETH, Asset.BRL),
                        new Market(Asset.LTC, Asset.BRL),
                        new Market(Asset.XRP, Asset.BRL),
                        new Market(Asset.WBX, Asset.BRL),
                        new Market(Asset.CHZ, Asset.BRL),
                        new Market(Asset.USDC, Asset.BRL),
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
                    Exchange = ExchangeName.MercadoBitcoin,
                    TimeFrame = TimeFrame.D1,
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
                    Exchange = ExchangeName.MercadoBitcoin,
                    DateTimeOffset = DateTimeUtils.CurrentUtcDateTimeOffset(),
                    Market = market,
                    Bids = from order in response.Output.Bids.ToList()
                           select new Order(market, Order.Types.Buy, order[1], order[0]),
                    Asks = from order in response.Output.Bids.ToList()
                           select new Order(market, Order.Types.Sell, order[1], order[0]),
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
                    Exchange = ExchangeName.MercadoBitcoin,
                    TimeFrame = TimeFrame.D1,
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
                                       String.Parse<Order.Types>(trade.Type),
                                       trade.Amount,
                                       trade.Price,
                                       new Guid(trade.Tid.GetHashCode().ToString(CultureInfo.InvariantCulture)),
                                       DateTimeUtils.TimestampToDateTimeOffset(trade.TimeStamp, false),
                                       Order.Statuses.Closed);

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
