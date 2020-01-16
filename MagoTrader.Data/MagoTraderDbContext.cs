using System;

namespace MagoTrader.Data
{
    public class MagoTraderDbContext : DbContext
    {
        public DbSet<OHLCV> OHLCV { get; set; } 
        
    }
}
