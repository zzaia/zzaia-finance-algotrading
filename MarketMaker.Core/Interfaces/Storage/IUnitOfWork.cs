using System;
using System.Threading.Tasks;

namespace MarketMaker.Core.Storage
{
    public interface IUnitOfWork : IDisposable
    {
        IOHLCVRepository OHLCV { get; }
        IExchangeRepository Exchange { get; }
        //IOrderRepository Order { get; }
        Task<int> CommitAsync();
    }
}