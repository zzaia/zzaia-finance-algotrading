using MagoTrader.Exchange.MercadoBitcoin.Private;
using MagoTrader.Exchange.MercadoBitcoin.Public;
using MagoTrader.Exchange.MercadoBitcoin.Trade;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Exchange.MercadoBitcoin
{
    public interface IMercadoBitcoinExchange
    {
        IPublicApiClient PublicClient { get; }
        IPrivateApiClient PrivateClient { get; }
        ITradeApiClient TradeClient { get; }
    }
}
