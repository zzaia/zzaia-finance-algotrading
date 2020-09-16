using MarketMaker.Exchange.MercadoBitcoin.Private;
using MarketMaker.Exchange.MercadoBitcoin.Public;
using MarketMaker.Exchange.MercadoBitcoin.Trade;

namespace MarketMaker.Exchange.MercadoBitcoin
{
    public interface IMercadoBitcoinExchange
    {
        IPublicApiClient PublicClient { get; }
        IPrivateApiClient PrivateClient { get; }
        ITradeApiClient TradeClient { get; }
    }
}
