using Microsoft.EntityFrameworkCore;
using MagoTrader.Data.Configurations;
using MagoTrader.Core.Models;
using System;

namespace MagoTrader.Data
{
    public class MagoTraderDbContext : DbContext
    {
        //public DbSet<OHLCV> OHLCV { get; set; } 
        public DbSet<Exchange> Exchanges { get; set; } 
        
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
