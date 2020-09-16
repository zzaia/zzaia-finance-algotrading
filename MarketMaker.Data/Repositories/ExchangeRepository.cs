using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MarketMaker.Core.Models;
using MarketMaker.Core.Repositories;
using MarketMaker.Core.Exchange;

namespace MarketMaker.Data.Repositories
{
    public class ExchangeRepository : Repository<IExchange>, IExchangeRepository
    {
        public ExchangeRepository(MarketMakerDbContext context) 
            : base(context)
        { }
        /*
        public async Task<IEnumerable<OHLCV>> GetAllWithArtistAsync()
        {
            return await MyMusicDbContext.Musics
                .Include(m => m.Artist)
                .ToListAsync();
        }

        public async Task<Music> GetWithArtistByIdAsync(int id)
        {
            return await MyMusicDbContext.Musics
                .Include(m => m.Artist)
                .SingleOrDefaultAsync(m => m.Id == id);;
        }

        public async Task<IEnumerable<Music>> GetAllWithArtistByArtistIdAsync(int artistId)
        {
            return await MyMusicDbContext.Musics
                .Include(m => m.Artist)
                .Where(m => m.ArtistId == artistId)
                .ToListAsync();
        }
        
        private MyMusicDbContext MyMusicDbContext
        {
            get { return Context as MyMusicDbContext; }
        }
        */
    }
}