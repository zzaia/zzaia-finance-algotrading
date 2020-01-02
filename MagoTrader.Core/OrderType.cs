namespace MagoTrader.Core
{
    public enum OrderType
    {
    /*  ========================================================================================================
        A order type for use within trades 
        Order Types: -> HOLD
                     -> LIMIT_BUY: Buy order at a given limit price, may not be completed; 
                     -> LIMIT_SELL: Sell order at a given limit price, may not be completed; 
                     -> MARKET_BUY: Buy order immediately at current market price, can suffer price slippage;
                     -> MARKET_SELL: Sell order immediately at current market price, can suffer slippage;
                     -> STOP_LOSS: Trigger a market order when "stopPrice" are crossed from above;
                     -> TAKE_PROFIT: Trigger a market order when "stopPrice" are crossed from below;                     
                     -> STOP_LOSS_LIMIT: Trigger a limit order when "stopPrice" are crossed from above;
                     -> TAKE_PROFIT_LIMIT: Trigger a limit order when "stopPrice" are crossed from below;
        ======================================================================================================= */
    HOLD,
    MARKET_BUY,
    LIMIT_SELL,
    MARKET_SELL,
    STOP_LOSS,
    TAKE_PROFIT,
    STOP_LOSS_LIMIT,
    TAKE_PROFIT_LIMIT
    }
}
