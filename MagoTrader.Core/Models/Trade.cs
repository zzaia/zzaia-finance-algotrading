using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MagoTrader.Core.Models;

namespace MagoTrader.Core
{
    public class Trade 
    {
        /*  ======================================================================================================================
           
            ====================================================================================================================== */
        public Order Order;
        public Order StopOrder;
        public Trade(string ticker, OrderType type, double amount = null, double? price = null, double? stopPrice = null, DateTime? validity = null)//, DateTime validity = default(DateTime))
        {

            Orders = new Order(type,validity);
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
