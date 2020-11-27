using MarketIntelligency.Core.Utils;
using System;

namespace MarketIntelligency.Core.Models.MarketAgregate
{
    /// <summary>
    /// Represents a financial order, that can be partially or completely fulfilled.
    /// </summary>
    public class Order
    {
        public Guid Id { get; set; }
        public Market Market { get; private set; }
        public decimal Price { get; private set; }
        public decimal Quantity { get; private set; }
        public string Status { get; set; }
        public string Type { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public OperationInfo OperationInfo { get; private set; }

        public Order(Market market, string type,
                                    decimal quantity,
                                    decimal price,
                                    Guid? id = null,
                                    DateTimeOffset? createdAt = null,
                                    string status = Statuses.Created)
        {
            Type = Types.IsValid(type) ? type : throw new ArgumentException(nameof(type));
            Status = Types.IsValid(status) ? status : throw new ArgumentException(nameof(status));
            Market = market ?? throw new ArgumentException(nameof(market));
            CreatedAt = createdAt ?? DateTimeUtils.CurrentLocalDateTimeOffset();
            Id = id ?? Guid.NewGuid();
            Quantity = quantity;
            Price = price;
        }

        public Order(Market market, OperationInfo operationInfo,
                                    decimal quantity,
                                    decimal price,
                                    Guid? id = null,
                                    DateTimeOffset? createdAt = null,
                                    string status = Statuses.Created)
        {
            Market = market ?? throw new ArgumentException(nameof(market));
            OperationInfo = operationInfo ?? throw new ArgumentException(nameof(operationInfo));
            Status = Types.IsValid(status) ? status : throw new ArgumentException(nameof(status));
            CreatedAt = createdAt ?? DateTimeUtils.CurrentLocalDateTimeOffset();
            Id = id ?? Guid.NewGuid();
            Quantity = quantity;
            Price = price;
        }


        public class Statuses
        {
            /* ====================================================================================
                 A order status
                Order Status: -> CREATED: Order only created by client or by strategy algorithm;
                              -> PLACED: Order placed in exchange by client or by algorithm;
                              -> CANCELED: Order canceled by client, algorithm or by exchange;
                              -> OPEN: Order still waiting to be completed by exchange;  
                              -> CLOSED: Order fullfilled and closed by exchange;          
               ==================================================================================== */

            public const string Created = "created";
            public const string Placed = "placed";
            public const string Canceled = "canceled";
            public const string Open = "open";
            public const string Closed = "closed";
            public static bool IsValid(string type)
            {
                return Canceled.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Placed.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Canceled.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Open.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Closed.Equals(type, StringComparison.OrdinalIgnoreCase);
            }
        }

        public class Types
        {
            /*  ========================================================================================================
                A order types
                Order Types: -> HOLD
                             -> BUY: Generic buy order at a given price, may not be completed; 
                             -> SELL: Generic sell order at a given price, may not be completed; 
                             -> LIMIT_SELL: Sell order at a given limit price, may not be completed; 
                             -> MARKET_BUY: Buy order by amount immediately at current market price, can suffer price slippage;
                             -> MARKET_SELL: Sell order by amount immediately at current market price, can suffer slippage;
                             -> STOP_LOSS: Trigger a market order when "stopPrice" are crossed from above;
                             -> TAKE_PROFIT: Trigger a market order when "stopPrice" are crossed from below;                     
                             -> STOP_LOSS_LIMIT: Trigger a limit order when "stopPrice" are crossed from above;
                             -> TAKE_PROFIT_LIMIT: Trigger a limit order when "stopPrice" are crossed from below;
                ======================================================================================================= */

            public const string Hold = "hold";
            public const string Buy = "buy";
            public const string Sell = "sell";
            public const string LimitBuy = "limit buy";
            public const string LimitSell = "limit sell";
            public const string MarketBuy = "market buy";
            public const string MarketSell = "market sell";
            public const string StopLoss = "stop loss";
            public const string TakeProfit = "take profit";
            public const string StopLossLimit = "stop loss limit";
            public const string TakeProfitLimit = "take profit limit";
            public static bool IsValid(string type)
            {
                return Hold.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Buy.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || Sell.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || LimitBuy.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || LimitSell.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || MarketBuy.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || MarketSell.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || StopLoss.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || TakeProfit.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || StopLossLimit.Equals(type, StringComparison.OrdinalIgnoreCase)
                    || TakeProfitLimit.Equals(type, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    /*
    public class Order2 : TimeData
    {
       /*  ======================================================================================================================
           A Order object for use within trading object
           Args: -> Ticker: The asset ticker, ex: BTC/USD;
                 -> Amount: The amount of ticker asset to be traded;
                 -> Price: The price paid per asset unit in limit orders.
                 -> Type: The type of order executed (0 = HOLD, 1=LIMIT_BUY, 2=MARKET_BUY, 3=LIMIT_SELL, 4=MARKET_SELL);
                 -> Status : The order status, indicate 
                 -> Validity: The period in which the trade will be consider valid [ms];
                 -> StopPrice : Price in which the STOP_LOSS or TAKE_PROFIT type of trade will be triggered;
       ====================================================================================================================== 
        private SortedList<DateTime, OrderStatusEnum> _status;
        [Required]
        public int Id { get; set; }
        [Required]
        public Exchange Exchange{get; set;}
        [Required, StringLength(9)]
        public Market Ticker { get; private set; }
        [Required]
        public OrderTypeEnum Type { get; private set; }
        public double? Amount { get; private set; }
        public Decimal? Price { get; private set; }
        public Decimal? StopPrice { get; private set; }
        public DateTime? Validity { get; private set; }

        public Decimal Cost { get; set; }
        public Decimal Fee { get; set; }
        public double Filled { get; set; }
        public double Remaining { get; set; }

        public OrderStatusEnum Status
        {
            get
            {
                return _status.Last().Value;
            }
            set
            {
                _status.Add(CurrentDateTime, value);
            }
        }
        public Order2(Market ticker, OrderTypeEnum type, double? amount, Decimal? price, Decimal? stopPrice, DateTime? validity)//, DateTime validity = default(DateTime))
        {
            _status = new SortedList<DateTime, OrderStatusEnum>();
            Ticker = ticker;
            Type = type;
            Status = OrderStatusEnum.CREATED;
            Amount = amount;
            Price = price;
            StopPrice = stopPrice;
            Validity = validity;
        }

        public bool IsValid
        // Returns, whether the placed order still time valid
        {
            get
            {
                if (CurrentDateTime < _status.First().Key)
                {
                    return true;
                }
                return false;
            }
        }
   
        public OAuthToken()
        {
            created_at = DateTimeOffset.Now;
        }

        public string access_token { get; set; }
        public string token_type { get; set; }
        public int? expires_in { get; set; }
        public int? ext_expires_in { get; set; }
        public DateTimeOffset created_at { get; set; }

        public bool isExpired()
        {
            DateTimeOffset expires_at = this.created_at.AddSeconds(Convert.ToDouble(this.expires_in));
            return DateTimeOffset.Now.CompareTo(expires_at) >= 0;
        }

        
        public bool IsPlaced { get { return Order.Status == OrderStatus.PLACED; } }
        public bool IsCreated { get { return Order.Status == OrderStatus.CREATED; } }
        public bool IsCanceled { get { return Order.Status == OrderStatus.CANCELED; } }
        public bool IsOpen { get { return Order.Status == OrderStatus.OPEN; } }
        public bool IsHold { get { return Order.Type == OrderType.HOLD; } }
        public bool IsBuy { get { return Order.Type == OrderType.MARKET_BUY || Order.Type == OrderType.LIMIT_BUY; } }
        public bool IsSell
        {
            get
            {
                return  Order.Type == OrderType.MARKET_SELL ||
                        Order.Type == OrderType.LIMIT_SELL ||
                        Order.Type == OrderType.STOP_LOSS ||
                        Order.Type == OrderType.STOP_LOSS_LIMIT ||
                        Order.Type == OrderType.TAKE_PROFIT ||
                        Order.Type == OrderType.TAKE_PROFIT_LIMIT;
            }
        }

        public bool IsLimit
        {
            get
            {
                return Order.Type == OrderType.LIMIT_SELL ||
                       Order.Type == OrderType.LIMIT_BUY ||
                       Order.Type == OrderType.STOP_LOSS_LIMIT ||
                       Order.Type == OrderType.TAKE_PROFIT_LIMIT;
            }
        }

        public bool IsMarket
        {
            get
            {
                return Order.Type == OrderType.MARKET_SELL ||
                       Order.Type == OrderType.MARKET_BUY ||
                       Order.Type == OrderType.STOP_LOSS ||
                       Order.Type == OrderType.TAKE_PROFIT;
            }
        }
    }
    */
}