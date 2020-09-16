using MarketMaker.Core.Exchange;
using MarketMaker.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
//https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.metadata.builders.entitytypebuilder?view=efcore-3.1
namespace MarketMaker.Data.Configurations
{
    public class ExchangeConfiguration : IEntityTypeConfiguration<IExchange>
    {
        public void Configure(EntityTypeBuilder<IExchange> builder)
        {
            /*
            builder
                .HasKey(m => m.Exchange)
                .IsRequired();

            builder
                .Property(m => m.Id)
                .UseIdentityColumn();
                
            builder
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .HasOne(m => m.Artist)
                .WithMany(a => a.Musics)
                .HasForeignKey(m => m.ArtistId);

            builder
                .ToTable("Exchanges");
                */
        }
    }
}