using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private int userId { get => this.User.GetUserId(); }
        private readonly IEventService _eventService;
        private readonly IWebHostEnvironment _env;
        private readonly IAccountService _accountService;

        public EventsController(
            IEventService eventService,
            IWebHostEnvironment env,
            IAccountService accountService
        )
        {
            this._eventService = eventService;
            this._env = env;
            this._accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PageParams pageParams)
        {
            try
            {
                var evnts = await this._eventService.GetEvents(this.userId, 
                    pageParams, true);

                if (evnts == null) return NoContent();

                Response.AddPagination(
                    evnts.CurrentPage, evnts.PageSize, 
                    evnts.TotalCount, evnts.TotalPages
                );

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
                var evnt = await this._eventService.GetEventById(
                    this.userId, id, true);
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
                var evnt = await this._eventService.AddEvent(this.userId, model);
                if (evnt == null) return NoContent();

                return Ok(evnt);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPost("upload-image/{eventId}")]
        public async Task<IActionResult> UploadImage(int eventId)
        {
            try
            {
                var evnt = await this._eventService.GetEventById(this.userId, eventId);
                if (evnt == null) return NoContent();

                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    this.DeleteImage(evnt.ImageUri);
                    evnt.ImageUri = await this.SaveImage(file);
                }

                var evntResponse = await this._eventService.UpdateEvent(
                    this.userId, eventId, evnt);

                return Ok(evntResponse);
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
                var evnt = await this._eventService.UpdateEvent(
                    this.userId, id, model);
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
                var evnt = await this._eventService.GetEventById(
                    this.userId, id);
                if (evnt == null) return NoContent();

                string imageUri = evnt.ImageUri;
                bool isDeleted = await this._eventService.DeleteEvent(
                    this.userId, id);

                if (isDeleted && !String.IsNullOrEmpty(imageUri))
                {
                    this.DeleteImage(imageUri);
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Event Not Deleted.");
                }
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

        [NonAction]
        private void DeleteImage(string imageUri)
        {
            if(String.IsNullOrEmpty(imageUri)) return;
            
            var imagePath = Path.Combine(
                this._env.ContentRootPath, @"Resources/Images", imageUri
            );
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        [NonAction]
        private async Task<string> SaveImage(IFormFile file)
        {
            string imageName = new string(
                Path.GetFileNameWithoutExtension(file.FileName)
                    .Take(10).ToArray()
            ).Replace(" ", "-");

            imageName =
                $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(file.FileName)}";

            var imagePath = Path.Combine(
                this._env.ContentRootPath, @"Resources/Images", imageName
            );

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return imageName;
        }
    }
}
