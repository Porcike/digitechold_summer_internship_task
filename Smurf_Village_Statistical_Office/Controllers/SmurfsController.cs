using Microsoft.AspNetCore.Mvc;
using Smurf_Village_Statistical_Office.DTO.SmurfDtos;
using Smurf_Village_Statistical_Office.Services.General;
using Smurf_Village_Statistical_Office.Services.SmurfServices.ExportStrategies;
using Smurf_Village_Statistical_Office.Services.SmurfServices.General;

namespace Smurf_Village_Statistical_Office.Controllers
{
    [Route("stat")]
    [ApiController]
    public class SmurfsController(
        ISmurfService smurfService, 
        ExportService<ISmurfsExportStrategy> exportService) : ControllerBase
    {
        private readonly ISmurfService _smurfService = smurfService;

        private readonly ExportService<ISmurfsExportStrategy> _exportService = exportService;

        [HttpGet("Smurfs")]
        public async Task<IActionResult> List([FromQuery] SmurfFilterDto filter)
        {
            var smurfs = await _smurfService.GetAllAsync(filter);
            return Ok(smurfs);
        }

        [HttpGet("Smurfs/{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var smurf = await _smurfService.GetByIdAsnyc(id);
            return smurf == null ? NotFound() : Ok(smurf);
        }

        [HttpPost("Smurfs")]
        public async Task<IActionResult> Create([FromBody] CreateSmurfDto value)
        {
            try
            {
                var created = await _smurfService.InsertAsync(value);
                return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Smurfs")]
        public async Task<IActionResult> Update([FromBody] UpdateSmurfDto value)
        {
            try
            {
                await _smurfService.UpdateAsync(value);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("Smurfs/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _smurfService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("Smurfs/actions/export/{type}")]
        public async Task<IActionResult> ExportAll([FromRoute] string type)
        {
            try
            {
                var (bytes, contentType, fileName) = await _exportService.ExportAllAsync(type);
                return File(bytes, contentType, fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("Smurfs/{id}/actions/export/{type}")]
        public async Task<IActionResult> Export([FromRoute] int id, [FromRoute] string type)
        {
            try
            {
                var (bytes, contentType, fileName) = await _exportService.ExportByIdAsync(id, type);
                return File(bytes, contentType, fileName);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }
    }
}
