using MagoTrader.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
// https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.metadata.builders.entitytypebuilder?view=efcore-3.1
namespace MagoTrader.Data.Configurations
{
    public class OHLCVConfiguration : IEntityTypeConfiguration<OHLCV>
    {
        public void Configure(EntityTypeBuilder<OHLCV> builder)
        {
            /*
            builder
                .HasKey(m => m.Exchange);

            builder
                .HasOne(m => m.Ticker);

            builder
                .Property(m => m.DateTime)
                .IsRequired();

            builder
                .Property(m => m.Open);
            
            builder
                .Property(m => m.High);
                
            builder
                .Property(m => m.Low);
                
            builder
                .Property(m => m.Close);
                
            builder
                .Property(m => m.Volume);

            builder
                .HasOne(m => m.DateTime.TimeFrame)
                .WithMany(a => a.Bids)
                .HasColumnName("Bids");

            builder
                .HasOne(m => m.DateTime.TimeFrame)
                .WithMany(a => a.Asks)
                .HasColumnName("Asks");

            builder
                .ToTable("OHLCV");
            */
        }
    }
}