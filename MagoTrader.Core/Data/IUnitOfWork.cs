using System;
using System.Threading.Tasks;
using MagoTrader.Core.Repositories;

namespace MagoTrader.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IOHLCVRepository OHLCV { get; }
        IExchangeRepository Exchange { get; }
        //IOrderRepository Order { get; }
        Task<int> CommitAsync();
    }
}