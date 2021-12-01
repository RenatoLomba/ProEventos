using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contracts
{
    public interface IEventPersist
    {
        Task<PageList<Event>> GetEventsAsync(int userId, PageParams pageParams, 
            bool includeSpeakers = false);

        Task<Event> GetEventByIdAsync(
            int userId, int id, bool includeSpeakers = false);
    }
}
