using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        public IEnumerable<Event> _events = new Event[]
            {
                new Event() {
                    EventId = 1,
                    Theme = "Angular 11 e .NET 5",
                    Place = "Belo Horizonte",
                    Batch = "1º Lote",
                    PeopleQty = 250,
                    EventDate = DateTime.Now.AddDays(2).ToString("dd/MM/yyyy"),
                    ImageUri = "img.png"
                },
                new Event() {
                    EventId = 2,
                    Theme = "React 16 e Node 16",
                    Place = "São Paulo",
                    Batch = "2º Lote",
                    PeopleQty = 1540,
                    EventDate = DateTime.Now.AddDays(5).ToString("dd/MM/yyyy"),
                    ImageUri = "img2.png"
                },
            };
        public EventController()
        {

        }

        [HttpGet]
        public IEnumerable<Event> Get()
        {
            return this._events;
        }

        [HttpGet("{id}")]
        public Event GetById(int id)
        {
            return this._events.FirstOrDefault(e => e.EventId.Equals(id));
        }

        [HttpPost]
        public string Post()
        {
            return "post";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return $"put id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"delete id = {id}";
        }
    }
}
