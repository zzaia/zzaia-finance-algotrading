using MarketIntelligency.Core.Models.EnumerationAggregate;

namespace MarketIntelligency.Core.Interfaces.ExchangeAggregate
{
    public interface IExchangeSelector
    {
        IExchange SelectByName(ExchangeName exchangeName);
    }
}