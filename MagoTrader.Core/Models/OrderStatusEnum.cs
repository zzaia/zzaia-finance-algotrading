using System;

namespace MagoTrader.Core.Models
{
    public enum OrderStatusEnum
    {
    /* ====================================================================================
         A order status for use within trades
        Order Status: -> CREATED: Order only created by client or by strategy algorithm;
                      -> PLACED: Order placed in exchange by client or by algorithm;
                      -> CANCELED: Order canceled by client, algorithm or by exchange;
                      -> OPEN: Order still waiting to be completed by exchange;  
                      -> CLOSED: Order fullfilled and closed by exchange;          
       ==================================================================================== */
    CREATED,
    PLACED,
    CANCELED,
    OPEN,
    CLOSED
    } 
}
