using System;
using System.Threading.Tasks;
using MagoTrader.Core.Repositories;

namespace MagoTrader.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IOHLCVRepository OHLCV { get; }
        IOrderRepository Order { get; }
        Task<int> CommitAsync();
    }
}