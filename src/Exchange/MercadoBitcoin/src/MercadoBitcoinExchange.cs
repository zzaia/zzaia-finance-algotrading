using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using MarketIntelligency.Core.Utils;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Private;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Public;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Trade;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.MercadoBitcoin
{
    public partial class MercadoBitcoinExchange : IMercadoBitcoinExchange, IExchange
    {
        private readonly PublicApiClient _publicApiClient;
        private readonly PrivateApiClient _privateApiClient;
        private readonly TradeApiClient _tradeApiClient;
        private readonly ILogger<MercadoBitcoinExchange> _logger;
        private readonly TelemetryClient _telemetryClient;
        private readonly ClientCredential _tradeClientCredential;
        private readonly ClientCredential _privateClientCredential;

        /// <summary>
        /// Exchange client instance, selects api end-point based on parameter
        /// </summary>
        public MercadoBitcoinExchange(Action<ClientCredential> privateClientCredentials,
                                      Action<ClientCredential> tradeClientCredentials,
                                      ILogger<MercadoBitcoinExchange> logger,
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
            _publicApiClient = new PublicApiClient(clientFactory.CreateClient());
            _privateApiClient = new PrivateApiClient(clientFactory.CreateClient());
            _tradeApiClient = new TradeApiClient(clientFactory.CreateClient());
            _publicApiClient.SetBaseAddress(Information.Uris.WebApi.Public);
            _privateApiClient.SetBaseAddress(Information.Uris.WebApi.Private);
            _tradeApiClient.SetBaseAddress(Information.Uris.WebApi.Trade);
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
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
                        WebApi = new WebApiUris
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
                    LimitRate = new ExchangeLimitRate
                    {
                        Rate = 1
                    },
                    Options = new ExchangeOptions
                    {
                        HasWebApi = true,
                        HasWebSocket = false
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

        /// <summary>
        /// Fetch orderbook L1 for the specified market, returns a simplified orderbook by price aggregation.
        /// </summary>
        public async Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken)
        {
            Log.FetchOrderBook.Received(_logger);
            Log.FetchOrderBook.ReceivedAction(_telemetryClient);
            market = market ?? throw new ArgumentNullException(nameof(market));
            try
            {
                var response = await this.PublicClient.GetOrderBookAsync(market.Base.DisplayName, cancellationToken).ConfigureAwait(false);

                if (response.Success)
                {
                    var resultToReturn = new OrderBook
                    {
                        Exchange = ExchangeName.MercadoBitcoin,
                        DateTimeOffset = DateTimeUtils.CurrentUtcDateTimeOffset(),
                        Market = market,
                        Bids = from order in response.Output.Bids.ToList()
                               select new OrderBookLevel(Guid.NewGuid().ToString(), order[1], order[0]),
                        Asks = from order in response.Output.Asks.ToList()
                               select new OrderBookLevel(Guid.NewGuid().ToString(), order[1], order[0]),
                    };

                    return ObjectResultFactory.CreateSuccessResult(resultToReturn);
                }
                else
                {
                    var errorMessage = JsonSerializer.Serialize(response.ProblemDetails);
                    Log.FetchOrderBook.WithFailedResponse(_logger, errorMessage);
                    return ObjectResultFactory.CreateFailResult<OrderBook>();
                }
            }
            catch (OperationCanceledException)
            {
                Log.FetchOrderBook.WithOperationCanceled(_logger);
                return ObjectResultFactory.CreateFailResult<OrderBook>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Fetch orderbook L3 for the specified market, returns a simplified orderbook by price aggregation.
        /// </summary>
        public async Task<ObjectResult<OrderBook>> FetchL3OrderBookAsync(Market market, CancellationToken cancellationToken)
        {
            Log.FetchOrderBook.Received(_logger);
            Log.FetchOrderBook.ReceivedAction(_telemetryClient);
            market = market ?? throw new ArgumentNullException(nameof(market));

            try
            {
                var ticker = ToDataDomain(market);
                var response = await this.PrivateClient.GetCompleteOrderBookByTickerPairAsync(_privateClientCredential, ticker, true, cancellationToken).ConfigureAwait(false);

                if (response.Success && response.Output.Success)
                {
                    var resultToReturn = new OrderBook
                    {
                        Exchange = ExchangeName.MercadoBitcoin,
                        DateTimeOffset = DateTimeUtils.CurrentUtcDateTimeOffset(),
                        Market = market,
                        Bids = from order in response.Output.Data.Orderbook.Bids.ToList()
                               select new OrderBookLevel(Guid.NewGuid().ToString(), decimal.Parse(order.PriceLimit, Information.Culture.NumberFormat),
                                                                  decimal.Parse(order.Quantity, Information.Culture.NumberFormat)),
                        Asks = from order in response.Output.Data.Orderbook.Asks.ToList()
                               select new OrderBookLevel(Guid.NewGuid().ToString(), decimal.Parse(order.PriceLimit, Information.Culture.NumberFormat),
                                                                  decimal.Parse(order.Quantity, Information.Culture.NumberFormat)),
                    };

                    return ObjectResultFactory.CreateSuccessResult(resultToReturn);
                }
                else if (!response.Output.Success)
                {
                    var errorMessage = $"{response.Output.Code} - {response.Output.Message}";
                    Log.FetchOrderBook.WithFailedResponse(_logger, errorMessage);
                    return ObjectResultFactory.CreateFailResult<OrderBook>();
                }
                else
                {
                    var errorMessage = JsonSerializer.Serialize(response.ProblemDetails);
                    Log.FetchOrderBook.WithFailedResponse(_logger, errorMessage);
                    return ObjectResultFactory.CreateFailResult<OrderBook>();
                }
            }
            catch (OperationCanceledException)
            {
                Log.FetchOrderBook.WithOperationCanceled(_logger);
                return ObjectResultFactory.CreateFailResult<OrderBook>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        private static string ToDataDomain(Market market)
        {
            return $"{market.Quote.DisplayName}{market.Base.DisplayName}";
        }

        #endregion
    }
}