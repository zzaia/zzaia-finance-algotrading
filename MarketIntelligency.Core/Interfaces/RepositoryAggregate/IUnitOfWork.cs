using System;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Interfaces.RepositoryAggregate
{
    public interface IUnitOfWork : IDisposable
    {
        IOHLCVRepository OHLCV { get; }
        IExchangeInfoRepository ExchangeInfo { get; }
        IOrderRepository Order { get; }
        Task<int> CommitAsync();
    }
}