using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Implementations
{
    public class EventPersist : IEventPersist
    {
        private readonly ProEventosContext _context;

        public EventPersist(ProEventosContext context)
        {
            this._context = context;
        }

        public async Task<PageList<Event>> GetEventsAsync(int userId, 
            PageParams pageParams, bool includeSpeakers = false)
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

            query = query.AsNoTracking()
                .Where(ev => ev.UserId.Equals(userId))
                .Where(ev => 
                    ev.Theme.ToLower().Contains(pageParams.Term.ToLower()) ||
                    ev.Place.ToLower().Contains(pageParams.Term.ToLower()))
                .OrderBy(ev => ev.Id);

            var pageNumber = pageParams.PageNumber;
            var pageSize = pageParams.PageSize;

            return await PageList<Event>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Event> GetEventByIdAsync(int userId, 
            int id, bool includeSpeakers = false)
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
                .Where(ev => ev.Id.Equals(id) && ev.UserId.Equals(userId));

            return await query.FirstOrDefaultAsync();
        }
    }
}
