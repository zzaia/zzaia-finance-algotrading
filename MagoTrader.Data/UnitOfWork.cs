using System.Threading.Tasks;
using MarketMaker.Core;
using MarketMaker.Core.Repositories;
using MarketMaker.Data.Repositories;

namespace MarketMaker.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MagoTraderDbContext _context;
        
        private OHLCVRepository _ohlcvRepository;
        private ExchangeRepository _exchangeRepository;

        public UnitOfWork(MagoTraderDbContext context)
        {
            this._context = context;
        }

        public IOHLCVRepository OHLCV => _ohlcvRepository = _ohlcvRepository ?? new OHLCVRepository(_context);

        public IExchangeRepository Exchange => _exchangeRepository = _exchangeRepository ?? new ExchangeRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}