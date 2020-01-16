using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagoTrader.Core.Models;

namespace MagoTrader.Core.Repositories
{
    public interface IOHLCVRepository : IRepository<OHLCV>
    {
        Task<IEnumerable<OHLCV>> GetAllWithOHLCVAsync();
        Task<OHLCV> GetWithOHLCVByDateTimeAsync(DateTime dt);
        Task<IEnumerable<OHLCV>> GetAllWithOHLCVByOHLCVDateTimeAsync(int artistId);
    }
}