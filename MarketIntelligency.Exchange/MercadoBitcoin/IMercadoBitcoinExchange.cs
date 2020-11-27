using MarketIntelligency.Exchange.MercadoBitcoin.Private;
using MarketIntelligency.Exchange.MercadoBitcoin.Public;
using MarketIntelligency.Exchange.MercadoBitcoin.Trade;

namespace MarketIntelligency.Exchange.MercadoBitcoin
{
    public interface IMercadoBitcoinExchange
    {
        IPublicApiClient PublicClient { get; }
        IPrivateApiClient PrivateClient { get; }
        ITradeApiClient TradeClient { get; }
    }
}
