using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace MagoTrader.Core.Models
{

    public class Order : TimeData
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
        ====================================================================================================================== */
        private SortedList<DateTime, OrderStatus> _status;
        [Required]
        public int Id { get; set; }
        [Required, StringLength(7)]
        public string Ticker { get; private set; }
        [Required]
        public OrderType Type { get; private set; }
        public double Amount { get; private set; }
        public double Price { get; private set; }
        public double StopPrice { get; private set; }
        public DateTime Validity { get; private set; }

        public double Cost { get; set; }
        public double Fee { get; set; }
        public double Filled { get; set; }
        public double Remaining { get; set; }

        public OrderStatus Status
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
        public Order(string ticker, OrderType type, double? amount = null, double? price = null, double? stopPrice = null, DateTime? validity = null)//, DateTime validity = default(DateTime))
        {
            _status = new SortedList<DateTime, OrderStatus>();
            Ticker = ticker;
            Type = type;
            Status = OrderStatus.CREATED;
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
}