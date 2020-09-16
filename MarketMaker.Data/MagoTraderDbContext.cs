using Microsoft.EntityFrameworkCore;
using MarketMaker.Data.Configurations;
using MarketMaker.Core.Models;
using System;
using MarketMaker.Core.Exchange;

namespace MarketMaker.Data
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
