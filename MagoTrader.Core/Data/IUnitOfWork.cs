using System;
using System.Threading.Tasks;
using MarketMaker.Core.Repositories;

namespace MarketMaker.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IOHLCVRepository OHLCV { get; }
        IExchangeRepository Exchange { get; }
        //IOrderRepository Order { get; }
        Task<int> CommitAsync();
    }
}