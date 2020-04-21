using Microsoft.EntityFrameworkCore;
using MagoTrader.Data.Configurations;
using MagoTrader.Core.Models;
using System;
using MagoTrader.Core.Exchange;

namespace MagoTrader.Data
{
    public class MagoTraderDbContext : DbContext
    {
        //public DbSet<OHLCV> OHLCV { get; set; } 
        public DbSet<IExchange> Exchanges { get; set; } 
        
        public MagoTraderDbContext(DbContextOptions<MagoTraderDbContext> options):base(options)
        {

        }
        /*
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .ApplyConfiguration(new OHLCVConfiguration());

            builder
                .ApplyConfiguration(new ExchangeConfiguration());
        }
        */
    }
}
