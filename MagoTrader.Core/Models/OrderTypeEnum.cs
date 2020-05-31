namespace MagoTrader.Core.Models
{
    public enum OrderTypeEnum
    {
        /*  ========================================================================================================
            A order type for use within trades 
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
    HOLD,
    BUY,
    SELL,
    LIMIT_BUY,
    LIMIT_SELL,
    MARKET_BUY,
    MARKET_SELL,
    STOP_LOSS,
    TAKE_PROFIT,
    STOP_LOSS_LIMIT,
    TAKE_PROFIT_LIMIT
    }
}