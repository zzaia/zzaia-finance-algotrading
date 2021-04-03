
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Interfaces.ExchangeAggregate
{
    /// <summary>
    /// Describes all common public methods for all implemented exchanges under MarketIntelligency.Exchange namespace.
    /// </summary>
    public interface IExchange
    {
        static ExchangeInfo Information { get; }
        ExchangeInfo Info { get; }

        Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market, CancellationToken cancellationToken);
    }
}