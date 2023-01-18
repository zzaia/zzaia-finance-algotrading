using Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Private;
using Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Public;
using Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Trade;

namespace Zzaia.Finance.Exchange.MercadoBitcoin
{
    public interface IMercadoBitcoinExchange
    {
        IPublicApiClient PublicClient { get; }
        IPrivateApiClient PrivateClient { get; }
        ITradeApiClient TradeClient { get; }
    }
}
