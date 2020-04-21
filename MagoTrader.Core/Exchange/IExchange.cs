
namespace MagoTrader.Core.Exchange
{
    public interface IExchange
    {
        IPrivateApiClient Private { get; }
        IPublicApiClient Public { get; }
        ITradeApiClient Trade { get; }
        ExchangeInfo Info { get; }
    }
}