using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contracts
{
    public interface IEventPersist
    {
        Task<Event[]> GetEventsByThemeAsync(
            int userId, string theme, bool includeSpeakers = false);
        Task<Event[]> GetEventsAsync(int userId, bool includeSpeakers = false);
        Task<Event> GetEventByIdAsync(
            int userId, int id, bool includeSpeakers = false);
    }
}
