
using MagoTrader.Core.Models;
using MagoTrader.Core.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MagoTrader.Core.Exchange
{
    /// <summary>
    /// Describes all common public methods for all implemented exchanges under MagoTrader.Exchange namespace.
    /// </summary>
    public interface IExchange
    {
        ExchangeInfo Info { get; }

        Task<ObjectResult<OHLCV>> FetchDaySummaryAsync(Market market, DateTimeOffset dateTime);
        Task<ObjectResult<OHLCV>> FetchOHLCVAsync(Market market);
        Task<ObjectResult<OrderBook>> FetchOrderBookAsync(Market market);
        Task<ObjectResult<IEnumerable<Order>>> FetchTradesAsync(Market market);
    }
}