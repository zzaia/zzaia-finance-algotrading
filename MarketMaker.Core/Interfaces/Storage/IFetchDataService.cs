using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketMaker.Core.Exchange;
using MarketMaker.Core.Models;

namespace MarketMaker.Core.Storage
{
    public interface IFetchDataService
    {
        Task<OHLCV[]> GetDefaultDaySummaryAsync(DateTimeOffset startDate, ExchangeName exchangeName);

    }

}