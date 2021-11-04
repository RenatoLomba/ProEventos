using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IEventPersist
    {
        Task<Event[]> GetEventsByThemeAsync(string theme, bool includeSpeakers = false);
        Task<Event[]> GetEventsAsync(bool includeSpeakers = false);
        Task<Event> GetEventByIdAsync(int id, bool includeSpeakers = false);
    }
}
