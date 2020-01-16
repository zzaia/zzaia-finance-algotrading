using MagoTrader.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MagoTrader.Data.Configurations
{
    public class OHLCVConfiguration : IEntityTypeConfiguration<OHLCV>
    {
        public void Configure(EntityTypeBuilder<OHLCV> builder)
        {
            builder
                .HasKey(m => m.Id);

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();
                
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .HasOne(m => m.DateTime)
                .WithMany(a => a.Musics)
                .HasForeignKey(m => m.ArtistId);

            builder
                .ToTable("OHLCV");
        }
    }
}