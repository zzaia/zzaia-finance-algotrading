using System.Collections.Generic;
using System.Threading.Tasks;
using MagoTrader.Core.Models;

namespace MagoTrader.Core.Repositories
{
    public interface IOrderRepository : IRepository<OHLCV>
    {
        Task<IEnumerable<OHLCV>> GetAllWithOrderAsync();
        Task<OHLCV> GetWithOrderByIdAsync(int id);
        Task<IEnumerable<OHLCV>> GetAllWithOrderByOrderIdAsync(int artistId);
    }
}