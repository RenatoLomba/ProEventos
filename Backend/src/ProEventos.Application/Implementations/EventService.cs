using System;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Implementations
{
    public class EventService : IEventService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IEventPersist _eventPersist;
        private readonly IMapper _mapper;
        public EventService(
            IGeneralPersist generalPersist,
            IEventPersist eventPersist,
            IMapper mapper
        )
        {
            this._eventPersist = eventPersist;
            this._generalPersist = generalPersist;
            this._mapper = mapper;
        }

        public async Task<EventDto> AddEvent(int userId, EventDto model)
        {
            try
            {
                var evnt = this._mapper.Map<Event>(model);
                evnt.UserId = userId;

                this._generalPersist.Add<Event>(evnt);

                if (await this._generalPersist.SaveChangesAsync())
                {
                    return this._mapper.Map<EventDto>(
                        await this._eventPersist.GetEventByIdAsync(userId, evnt.Id)
                    );
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDto> UpdateEvent(int userId, 
            int eventId, EventDto model)
        {
            try
            {
                var evnt = await this._eventPersist.GetEventByIdAsync(
                    userId, eventId);
                if (evnt == null) return null;

                model.Id = evnt.Id;
                model.UserId = userId;

                _mapper.Map(model, evnt);

                this._generalPersist.Update<Event>(evnt);

                if (await this._generalPersist.SaveChangesAsync())
                {
                    return this._mapper.Map<EventDto>(
                        await this._eventPersist.GetEventByIdAsync(userId, evnt.Id)
                    );
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvent(int userId, int eventId)
        {
            try
            {
                var evnt = await this._eventPersist.GetEventByIdAsync(
                    userId, eventId);
                if (evnt == null) throw new Exception("Event Not Found");

                this._generalPersist.Delete(evnt);
                return await this._generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDto[]> GetEvents(int userId, 
            bool includeSpeakers = false)
        {
            try
            {
                var result = await this._eventPersist.GetEventsAsync(
                    userId, includeSpeakers);
                return this._mapper.Map<EventDto[]>(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDto> GetEventById(int userId, 
            int id, bool includeSpeakers = false)
        {
            try
            {
                return this._mapper.Map<EventDto>(
                    await this._eventPersist.GetEventByIdAsync(
                        userId, id, includeSpeakers)
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventDto[]> GetEventsByTheme(int userId, 
            string theme, bool includeSpeakers = false)
        {
            try
            {
                return this._mapper.Map<EventDto[]>(
                    await this._eventPersist.GetEventsByThemeAsync(
                        userId, theme, includeSpeakers)
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
