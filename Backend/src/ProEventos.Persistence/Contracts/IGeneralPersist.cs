using System.Threading.Tasks;

namespace ProEventos.Persistence.Contracts
{
    public interface IGeneralPersist
    {
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void DeleteRange<T>(T[] entityList) where T : class;
        Task<bool> SaveChangesAsync();
    }
}
