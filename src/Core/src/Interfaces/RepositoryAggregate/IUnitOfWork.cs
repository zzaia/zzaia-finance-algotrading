using System;
using System.Threading.Tasks;

namespace Zzaia.Finance.Core.Interfaces.RepositoryAggregate
{
    public interface IUnitOfWork : IDisposable
    {
        IOHLCVRepository OHLCV { get; }
        IExchangeInfoRepository ExchangeInfo { get; }
        IOrderRepository Order { get; }
        Task<int> CommitAsync();
    }
}