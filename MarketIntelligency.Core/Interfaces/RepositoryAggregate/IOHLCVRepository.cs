using MarketIntelligency.Core.Models.EnumerationAggregate;
using MarketIntelligency.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Interfaces.RepositoryAggregate
{
    public interface IOHLCVRepository : IRepository<OHLCV>
    {
        Task<IEnumerable<OHLCV>> GetAllOHLCVAsync(Market asset, ExchangeName exchange);
        Task<IEnumerable<OHLCV>> GetOHLCVAsync(TimeFrame tm, 
                                        DateTime start_time, 
                                        DateTime end_time,
                                        Asset asset, 
                                        ExchangeName exchange);
    }
}