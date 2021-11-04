using System.Threading.Tasks;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence.Implementations
{
    public class GeneralPersist : IGeneralPersist
    {
        private readonly ProEventosContext _context;

        public GeneralPersist(ProEventosContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            this._context.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            this._context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            this._context.Remove(entity);
        }

        public void DeleteRange<T>(T[] entityList) where T : class
        {
            this._context.RemoveRange(entityList);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await this._context.SaveChangesAsync()) > 0;
        }

        public void Update<T>(T entity, T entityToModify) where T : class
        {
            throw new System.NotImplementedException();
        }
    }
}
