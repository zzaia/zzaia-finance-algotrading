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
        public readonly DateTime CreatedDateTime;

        public Trade(string ticker, OrderType type, double amount, double price ) 
        {
            Ticker = ticker;
            Type   = type;
            Amount = amount;
            Price  = price;
            CreatedDateTime = CurrentDateTime;
        }
        

        public OrderStatus Status { get; set; }
        public int Id { get; set; }
        public double Cost { get; set; }
        public double Fee { get; set; }
        public double Filled { get; set; }
        public double Remaining { get; set; }

        public DateTime LastTradeDateTime { get; set; }


        public bool IsValid { get; set; }
        public bool IsCreated { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsPlaced { get; set; }
        public bool IsHold { get; set; }
    }
}
