using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Implementations
{
    public class EventPersist : IEventPersist
    {
        private readonly ProEventosContext _context;

        public EventPersist(ProEventosContext context)
        {
            this._context = context;
        }

        public async Task<Event[]> GetEventsAsync(bool includeSpeakers = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(ev => ev.Batches)
                .Include(ev => ev.SocialNetworks);

            if (includeSpeakers)
            {
                query = query
                    .Include(ev => ev.SpeakersEvents)
                    .ThenInclude(se => se.Speaker);
            }

            query = query.AsNoTracking().OrderBy(ev => ev.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Event[]> GetEventsByThemeAsync(string theme, bool includeSpeakers = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(ev => ev.Batches)
                .Include(ev => ev.SocialNetworks);

            if (includeSpeakers)
            {
                query = query
                    .Include(ev => ev.SpeakersEvents)
                    .ThenInclude(se => se.Speaker);
            }

            query = query
                .AsNoTracking()
                .OrderBy(ev => ev.Id)
                .Where(ev => ev.Theme.ToLower().Contains(theme.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Event> GetEventByIdAsync(int id, bool includeSpeakers = false)
        {
            IQueryable<Event> query = _context.Events
                .Include(ev => ev.Batches)
                .Include(ev => ev.SocialNetworks);

            if (includeSpeakers)
            {
                query = query
                    .Include(ev => ev.SpeakersEvents)
                    .ThenInclude(se => se.Speaker);
            }

            query = query
                .AsNoTracking()
                .OrderBy(ev => ev.Id)
                .Where(ev => ev.Id.Equals(id));

            return await query.FirstOrDefaultAsync();
        }
    }
}
