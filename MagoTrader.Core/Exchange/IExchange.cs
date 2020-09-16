
using MarketMaker.Core.Models;
using MarketMaker.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketMaker.Core.Exchange
{
    /// <summary>
    /// Describes all common public methods for all implemented exchanges under MarketMaker.Exchange namespace.
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