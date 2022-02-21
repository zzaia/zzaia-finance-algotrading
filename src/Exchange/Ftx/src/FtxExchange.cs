using Crypto.Websocket.Extensions.Core.OrderBooks.Models;
using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Exchange.Ftx
{
    public class FtxExchange : IFtxExchange, IExchange
    {
        public ExchangeInfo Info => throw new NotImplementedException();

        public Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void SetOrderBookSubscription(Market market)
        {
            throw new NotImplementedException();
        }

        public void SubscribeToOrderBook(Action<IList<IOrderBookChangeInfo>> onNext)
        {
            throw new NotImplementedException();
        }
    }
}
