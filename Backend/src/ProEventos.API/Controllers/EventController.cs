using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Data;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly DataContext _context;

        public EventController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return this._context.Events.ToArray();
        }

        [HttpGet("{id}")]
        public Event GetById(int id)
        {
            return this._context.Events.FirstOrDefault(e => e.EventId.Equals(id));
        }
    }
}
