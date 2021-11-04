using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contracts
{
    public interface IEventService
    {
        Task<Event> AddEvent(Event model);
        Task<Event> UpdateEvent(int eventId, Event model);
        Task<bool> DeleteEvent(int eventId);

        Task<Event[]> GetEvents(bool includeSpeakers = false);
        Task<Event[]> GetEventsByTheme(string theme, bool includeSpeakers = false);
        Task<Event> GetEventById(int id, bool includeSpeakers = false);
    }
}
