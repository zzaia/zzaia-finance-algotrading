using MagoTrader.Core.Models;

namespace MagoTrader.Core.Exchange
{
    public interface IExchangeSelector
    {
        IExchange GetByName(ExchangeNameEnum exchangeName);
    }
}