using System.Threading.Tasks;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.Application.Contracts
{
    public interface IEventService
    {
        Task<EventDto> AddEvent(int userId, EventDto model);
        Task<EventDto> UpdateEvent(int userId, int eventId, EventDto model);
        Task<bool> DeleteEvent(int userId, int eventId);

        Task<EventDto[]> GetEvents(int userId, bool includeSpeakers = false);
        Task<EventDto[]> GetEventsByTheme(int userId, 
            string theme, bool includeSpeakers = false);
        Task<EventDto> GetEventById(int userId, 
            int id, bool includeSpeakers = false);
    }
}
