using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Implementations
{
    public class BatchPersist : IBatchPersist
    {
        private readonly ProEventosContext _context;

        public BatchPersist(ProEventosContext context)
        {
            this._context = context;
        }

        public async Task<Batch[]> GetBatchesByEventIdAsync(int eventId)
        {
            IQueryable<Batch> query = _context.Batches
                .AsNoTracking()
                .OrderBy(bt => bt.Id)
                .Where(bt => bt.EventId.Equals(eventId));

            return await query.ToArrayAsync();
        }

        public async Task<Batch> GetBatchByIdAsync(int eventId, int batchId)
        {
            IQueryable<Batch> query = _context.Batches
                .AsNoTracking()
                .Where(bt => bt.EventId.Equals(eventId) &&
                    bt.Id.Equals(batchId));

            return await query.FirstOrDefaultAsync();
        }
    }
}
