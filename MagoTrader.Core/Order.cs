using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Linq;

namespace MagoTrader.Core
{
    public class Order : TimeLog
    {
        /*  ======================================================================================================================
            A Order object for use within trading object
            Args:   -> Type: The type of order executed (0 = HOLD, 1=LIMIT_BUY, 2=MARKET_BUY, 3=LIMIT_SELL, 4=MARKET_SELL);
                    -> Status : The order status, indicate 
        ====================================================================================================================== */
        private Stack<Dictionary<OrderStatus,DateTime>> _status;
        //private Stack<Datetime> _datetime;

        [Required]
        public OrderType Type { get; private set; }
        [Required]
        public OrderStatus Status { 
            get{
                var dict = _status.Peek();
                return dict.Keys.ToList()[0];
            } 
            set{
                var dict = new Dictionary<OrderStatus,DateTime>();
                dict.Add(value,CurrentDateTime);
                _status.Push(dict);
            } }
        public Order( OrderType type )
        {
            _status = new Stack<Dictionary<OrderStatus,DateTime>>();
            //_datetime = new Stack<Status>();
            Type = type;
            Status = OrderStatus.CREATED;
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
        prop
    }
}