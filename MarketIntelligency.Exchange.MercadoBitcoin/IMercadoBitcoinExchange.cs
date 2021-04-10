using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Private;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Public;
using MarketIntelligency.Exchange.MercadoBitcoin.WebApi.Trade;

namespace MarketIntelligency.Exchange.MercadoBitcoin
{
    public interface IMercadoBitcoinExchange
    {
        IPublicApiClient PublicClient { get; }
        IPrivateApiClient PrivateClient { get; }
        ITradeApiClient TradeClient { get; }
    }
}
