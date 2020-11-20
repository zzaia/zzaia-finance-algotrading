using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketMaker.Core.Models;

namespace MarketMaker.Core.Storage
{
    public interface IOHLCVRepository : IRepository<OHLCV>
    {
        /*
        //Task<IEnumerable<OHLCV>> GetAllWithOHLCVAsync();
        //Task<OHLCV> GetOHLCVByDateTimeAsync(DateTime dt);
        Task<IEnumerable<OHLCV>> GetAllOHLCVAsync(AssetTicker asset, Exchange exchange);
        Task<IEnumerable<OHLCV>> GetOHLCVAsync(TimeFrame tm, 
                                        DateTime start_time, 
                                        DateTime end_time,
                                        AssetTicker asset, 
                                        Exchange exchange);
        */
    }
}