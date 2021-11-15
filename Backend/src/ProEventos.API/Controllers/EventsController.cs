using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Domain;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            this._eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string theme = null)
        {
            try
            {
                EventDto[] evnts = null;

                if (!String.IsNullOrEmpty(theme))
                {
                    evnts = await this._eventService.GetEventsByTheme(theme, true);
                }
                else
                {
                    evnts = await this._eventService.GetEvents(true);
                }

                if (evnts == null) return NoContent();

                return Ok(evnts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var evnt = await this._eventService.GetEventById(id, true);
                if (evnt == null) return NoContent();

                return Ok(evnt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventDto model)
        {
            try
            {
                var evnt = await this._eventService.AddEvent(model);
                if (evnt == null) return NoContent();

                return Ok(evnt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EventDto model)
        {
            try
            {
                var evnt = await this._eventService.UpdateEvent(id, model);
                if (evnt == null) return NoContent();

                return Ok(evnt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return await this._eventService.DeleteEvent(id) ?
                    Ok(true) :
                    BadRequest("Event Not Deleted.");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Event Not Found")
                {
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
