using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MagoTrader.Core.Models;
using MagoTrader.Core.Repositories;

namespace MagoTrader.Data.Repositories
{
    public class OHLCVRepository : Repository<OHLCV>, IOHLCVRepository
    {
        public OHLCVRepository(MagoTraderDbContext context) 
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