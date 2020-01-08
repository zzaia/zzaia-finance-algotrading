using System;
using System.ComponentModel.DataAnnotations;


namespace MagoTrader.Core
{
    public class Trade 
    {
        /*  ======================================================================================================================
            A trade object for use within trading environments 
            Args: -> ticker: The asset ticker, ex: BTC/USD;
                  -> amount: The amount of ticker asset to be traded;
                  -> price: The price paid per asset unit in limit orders.

            Params: -> 'validity': The period in which the trade will be consider valid [ms];
                    -> 'test': Check if it's valid but don't actually place it, [Binance Only][bool];
                    -> 'stopPrice' : Price in which the STOP_LOSS or TAKE_PROFIT type of trade will be triggered;
            ====================================================================================================================== */
        [Required]
        public Order Order { get;  }
        public Trade(string ticker, OrderType type, double amount, double price, DateTime validity)//, DateTime validity = unknown)
        {

            Order = new Order(type,validity);
        }

        

        public bool IsPlaced { get { return Order.Status == OrderStatus.PLACED; } }
        public bool IsCreated { get { return Order.Status == OrderStatus.CREATED; } }
        public bool IsCanceled { get { return Order.Status == OrderStatus.CANCELED; } }
        public bool IsOpen { get { return Order.Status == OrderStatus.OPEN; } }
        public bool IsValid { get { return Order.IsValid; } }
        public bool IsHold { get { return Order.Type == OrderType.HOLD; } }
        public bool IsBuy { get { return Order.Type == OrderType.MARKET_BUY || Order.Type == OrderType.LIMIT_BUY; } }
        public bool IsSell
        {
            get
            {
                return Order.Type == OrderType.MARKET_SELL ||
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
}
