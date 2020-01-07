using System;
using System.ComponentModel.DataAnnotations;


namespace MagoTrader.Core
{
    public class Trade : TimeLog
    {
        /*  ======================================================================================================================
            A trade object for use within trading environments 
            Args: -> ticker: The asset ticker, ex: BTC/USD;
                  -> order_type: The type of order executed (0 = HOLD, 1=LIMIT_BUY, 2=MARKET_BUY, 3=LIMIT_SELL, 4=MARKET_SELL);
                  -> amount: The amount of ticker asset to be traded;
                  -> price: The price paid per asset unit in limit orders.

            Params: -> 'validity': The period in which the trade will be consider valid [ms];
                    -> 'test': Check if it's valid but don't actually place it, [Binance Only][bool];
                    -> 'stopPrice' : Price in which the STOP_LOSS or TAKE_PROFIT type of trade will be triggered;
            ====================================================================================================================== */
        
        [Required, StringLength(7)]
        public readonly string Ticker;
        [Required]
        public readonly OrderType Type;
        [Required]
        public readonly double Amount;
        [Required]
        public readonly double Price;
        [Required]
        public readonly DateTime DateTimeFromCreation;
        public Trade(string ticker, OrderType type, double amount, double price)
        {
            Ticker = ticker;
            Type = type;
            Amount = amount;
            Price = price;
            DateTimeFromCreation = CurrentDateTime;
            Status = OrderStatus.CREATED;
        }

        public OrderStatus Status { get; set; }
        public int Id { get; set; }
        public double Cost { get; set; }
        public double Fee { get; set; }
        public double Filled { get; set; }
        public double Remaining { get; set; }
        public DateTime LastTradeDateTime { get; set; }
        public DateTime LastDateTimeToBeValid { get; set; }

        public bool IsPlaced { get { return Status == OrderStatus.PLACED; } }
        public bool IsCreated { get { return Status == OrderStatus.CREATED; } }
        public bool IsCanceled { get { return Status == OrderStatus.CANCELED; } }
        public bool IsOpen { get { return Status == OrderStatus.OPEN; } }
        public bool IsHold { get { return Type == OrderType.HOLD; } }
        public bool IsBuy { get { return Type == OrderType.MARKET_BUY || Type == OrderType.LIMIT_BUY; } }
        public bool IsSell
        {
            get
            {
                return Type == OrderType.MARKET_SELL ||
                    Type == OrderType.LIMIT_SELL ||
                    Type == OrderType.STOP_LOSS ||
                    Type == OrderType.STOP_LOSS_LIMIT ||
                    Type == OrderType.TAKE_PROFIT ||
                    Type == OrderType.TAKE_PROFIT_LIMIT;
            }
        }

        public bool IsLimit
        {
            get
            {
                return Type == OrderType.LIMIT_SELL ||
                       Type == OrderType.LIMIT_BUY ||
                       Type == OrderType.STOP_LOSS_LIMIT ||
                       Type == OrderType.TAKE_PROFIT_LIMIT;
            }
        }

        public bool IsMarket
        {
            get
            {
                return Type == OrderType.MARKET_SELL ||
                       Type == OrderType.MARKET_BUY ||
                       Type == OrderType.STOP_LOSS ||
                       Type == OrderType.TAKE_PROFIT;
            }
        }
        public bool IsValid
        // Returns, whether the placed order still time valid
        {
            get
            {
                if (LastDateTimeToBeValid != null)
                {
                    if(CurrentDateTime < LastDateTimeToBeValid ){
                        return true;

                    }
                }
                return true;
            }
        }

    }
}
