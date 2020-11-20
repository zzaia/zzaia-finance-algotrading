using System.Collections.Generic;
using System.Threading.Tasks;
using MarketMaker.Core.Models;

namespace MarketMaker.Core.Storage
{
    public interface IOrderRepository : IRepository<OHLCV>
    {
        Task<IEnumerable<OHLCV>> GetAllWithOrderAsync();
        Task<OHLCV> GetWithOrderByIdAsync(int id);
        Task<IEnumerable<OHLCV>> GetAllWithOrderByOrderIdAsync(int artistId);
    }
}