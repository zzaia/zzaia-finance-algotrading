using Zzaia.Finance.Core.Models.EnumerationAggregate;

namespace Zzaia.Finance.Core.Interfaces.ExchangeAggregate
{
    public interface IExchangeSelector
    {
        IExchange SelectByName(ExchangeName exchangeName);
    }
}