using Zzaia.Finance.Core.Models.ExchangeAggregate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zzaia.Finance.Core.Interfaces.RepositoryAggregate
{
    public interface IExchangeInfoRepository : IRepository<ExchangeInfo>
    {
        Task<IEnumerable<ExchangeInfo>> GetAllWithOrderAsync();
        Task<ExchangeInfo> GetWithExchangeByIdAsync(int id);
    }
}