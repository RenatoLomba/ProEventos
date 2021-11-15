using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Contracts
{
    public interface IEventService
    {
        Task<EventDto> AddEvent(EventDto model);
        Task<EventDto> UpdateEvent(int eventId, EventDto model);
        Task<bool> DeleteEvent(int eventId);

        Task<EventDto[]> GetEvents(bool includeSpeakers = false);
        Task<EventDto[]> GetEventsByTheme(string theme, bool includeSpeakers = false);
        Task<EventDto> GetEventById(int id, bool includeSpeakers = false);
    }
}
