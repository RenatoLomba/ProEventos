using System;
using System.Threading.Tasks;
using ProEventos.Application.Contracts;
using ProEventos.Domain;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Application.Implementations
{
    public class EventService : IEventService
    {
        private readonly IGeneralPersist _generalPersist;
        private readonly IEventPersist _eventPersist;
        public EventService(
            IGeneralPersist generalPersist,
            IEventPersist eventPersist
        )
        {
            this._eventPersist = eventPersist;
            this._generalPersist = generalPersist;
        }

        public async Task<Event> AddEvent(Event model)
        {
            try
            {
                this._generalPersist.Add(model);
                if (await this._generalPersist.SaveChangesAsync())
                {
                    return await this._eventPersist.GetEventByIdAsync(model.Id);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Event> UpdateEvent(int eventId, Event model)
        {
            try
            {
                var evnt = await this._eventPersist.GetEventByIdAsync(eventId);
                if (evnt == null) return null;

                model.Id = evnt.Id;

                this._generalPersist.Update(model);
                if (await this._generalPersist.SaveChangesAsync())
                {
                    return await this._eventPersist.GetEventByIdAsync(model.Id);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvent(int eventId)
        {
            try
            {
                var evnt = await this._eventPersist.GetEventByIdAsync(eventId);
                if (evnt == null) throw new Exception("Event Not Found");

                this._generalPersist.Delete(evnt);
                return await this._generalPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Event[]> GetEvents(bool includeSpeakers = false)
        {
            try
            {
                return await this._eventPersist.GetEventsAsync(includeSpeakers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Event> GetEventById(int id, bool includeSpeakers = false)
        {
            try
            {
                return await this._eventPersist.GetEventByIdAsync(id, includeSpeakers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Event[]> GetEventsByTheme(string theme, bool includeSpeakers = false)
        {
            try
            {
                return await this._eventPersist
                    .GetEventsByThemeAsync(theme, includeSpeakers);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
