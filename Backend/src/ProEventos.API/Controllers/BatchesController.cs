using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contracts;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchService _batchService;
        public BatchesController(IBatchService batchService)
        {
            this._batchService = batchService;
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> Get(int eventId)
        {
            try
            {
                var batches = await this._batchService.GetBatchesByEventId(
                    eventId
                );

                if (batches == null) return NoContent();

                return Ok(batches);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpPut("{eventId}")]
        public async Task<IActionResult> Put(int eventId, [FromBody] BatchDto[] models)
        {
            try
            {
                var batches = await this._batchService.SaveBatch(
                    eventId, models
                );
                if (batches == null) return NoContent();

                return Ok(batches);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Trying to update an unexisting batch")
                {
                    return BadRequest(ex.Message);
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }

        [HttpDelete("{eventId}/{batchId}")]
        public async Task<IActionResult> Delete(int eventId, int batchId)
        {
            try
            {
                return await this._batchService.DeleteBatch(eventId, batchId) ?
                    Ok(true) :
                    BadRequest("Batch Not Deleted.");
            }
            catch (Exception ex)
            {
                if (ex.Message == "Batch Not Found")
                {
                    return NoContent();
                }

                return StatusCode(StatusCodes.Status500InternalServerError,
                    ex.Message);
            }
        }
    }
}
