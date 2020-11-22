using MarketIntelligency.Core.Models.MarketAgregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Interfaces.RepositoryAggregate
{
    public interface IOrderRepository : IRepository<OHLCV>
    {
        Task<IEnumerable<OHLCV>> GetAllWithOrderAsync();
        Task<OHLCV> GetWithOrderByIdAsync(int id);
        Task<IEnumerable<OHLCV>> GetAllWithOrderByOrderIdAsync(int artistId);
    }
}