using MarketMaker.Core.Models;

namespace MarketMaker.Core.Exchange
{
    public interface IExchangeSelector
    {
        IExchange GetByName(ExchangeNameEnum exchangeName);
    }
}