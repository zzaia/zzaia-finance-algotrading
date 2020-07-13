using MagoTrader.Exchange.MercadoBitcoin.Private;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Exchange.MercadoBitcoin.Trade;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public interface IMercadoBitcoinExchange
    {
        IPublicApiClient PublicClient { get; }
        IPrivateApiClient PrivateClient { get; }
        ITradeApiClient TradeClient { get; }
    }
}
