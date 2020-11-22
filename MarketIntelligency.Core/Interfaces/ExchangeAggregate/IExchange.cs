
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;
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

        Task<ObjectResult<OHLCV>> FetchDaySummaryAsync(Market market, DateTimeOffset dateTime);
        Task<ObjectResult<OHLCV>> FetchOHLCVAsync(Market market);
        Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market);
        Task<ObjectResult<IEnumerable<Order>>> FetchTradesAsync(Market market);
    }
}