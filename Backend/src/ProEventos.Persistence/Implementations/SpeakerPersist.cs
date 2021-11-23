using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;

namespace ProEventos.Persistence.Implementations
{
    public class SpeakerPersist
    {
        private readonly ProEventosContext _context;

        public SpeakerPersist(ProEventosContext context)
        {
            this._context = context;
        }

        public async Task<Speaker[]> GetSpeakersAsync(bool includeEvents = false)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(sp => sp.SocialNetworks);

            if (includeEvents)
            {
                query = query
                    .Include(sp => sp.SpeakersEvents)
                    .ThenInclude(se => se.Event);
            }

            query = query.AsNoTracking().OrderBy(sp => sp.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Speaker[]> GetSpeakersByNameAsync(string name, bool includeEvents = false)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(sp => sp.SocialNetworks)
                .Include(sp => sp.User);

            if (includeEvents)
            {
                query = query
                    .Include(sp => sp.SpeakersEvents)
                    .ThenInclude(se => se.Event);
            }

            query = query
                .AsNoTracking()
                .OrderBy(sp => sp.Id)
                .Where(sp =>
                    sp.User.FullName.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Speaker> GetSpeakerByIdAsync(int id, bool includeEvents = false)
        {
            IQueryable<Speaker> query = _context.Speakers
                .Include(sp => sp.SocialNetworks);

            if (includeEvents)
            {
                query = query
                    .Include(sp => sp.SpeakersEvents)
                    .ThenInclude(se => se.Event);
            }

            query = query
                .AsNoTracking()
                .OrderBy(sp => sp.Id)
                .Where(sp => sp.Id.Equals(id));

            return await query.FirstOrDefaultAsync();
        }
    }
}
